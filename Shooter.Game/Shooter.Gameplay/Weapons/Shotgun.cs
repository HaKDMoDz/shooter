using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Core.Xna.Extensions;
using Shooter.Gameplay.Claims;
using Shooter.Gameplay.Weapons.Projectiles;

namespace Shooter.Gameplay.Weapons
{
    public class Shotgun : Weapon
    {
        private Body body;
        private float projectileSpeed = 35f;
        private IClaimer claimer;
        public Vector2 Position { get { return this.body.Position; } set { this.body.Position = value; } }
        private static readonly Random Random = new Random();

        private DateTime lastFire = DateTime.MinValue;

        public Shotgun(Engine engine)
            : base(engine)
        {
        }

        public void Reload(Unit unit)
        {
        }

        public void Fire(IFireRequest request)
        {
            var now = DateTime.UtcNow;

            if (now - lastFire < TimeSpan.FromMilliseconds(400))
            {
                return;
            }

            lastFire = now;

            for (int i = 0; i <= 10; i++)
            {
                var newShot = new Shot(this.Engine, request.Player);
                newShot.Initialize().Attach();
                var offset = (Random.NextDouble() - 0.5) * MathHelper.PiOver4;
                var speedChange = Random.NextDouble() + 1;
                var direction = (this.body.Rotation + (float) offset).RadiansToDirection();

                newShot.Velocity = direction * this.projectileSpeed * (float) speedChange + this.claimer.LinearVelocity;
                newShot.Rotation = this.body.Rotation;
                newShot.Position = this.body.Position + direction * 2f;
            }

            const float kickback = -25;
            this.Kickbacks.OnNext(this.body.Rotation.RadiansToDirection() * kickback);
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateRectangle(this.Engine.World, 0.5f, 0.5f, 1f);
            this.body.IsSensor = true;
            this.body.UserData = this;

            disposables.Add(this.body);

            disposables.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.claimer != null)
                                .Subscribe(this.LinkPhysics));

            disposables.Add(this.FireRequests.Subscribe(this.Fire));
            disposables.Add(this.ReloadRequests.Subscribe(this.Reload));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.claimer = null;
            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.claimer == null)
                                .Select(x => x.FixtureB.Body.UserData)
                                .OfType<IClaimer>()
                                .Subscribe(this.Claim));


            attachments.Add(this.ClaimRequests
                                .Where(x => this.claimer == null)
                                .Subscribe(this.Claim));
        }

        private void LinkPhysics(EngineTime time)
        {
            this.body.Position = this.claimer.Position;
            this.body.Rotation = this.claimer.Rotation;
        }

        private void Claim(IClaimer owner)
        {
            this.Detach();
            this.claimer = owner;
            this.claimer.Claims.OnNext(this);
        }
    }
}
