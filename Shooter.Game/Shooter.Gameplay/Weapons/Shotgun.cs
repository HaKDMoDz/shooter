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

        public int magazineSize = 200;

        public int magazineCount = 200;
        private DateTime lastFire = DateTime.MinValue;

        public Shotgun(Engine engine)
            : base(engine)
        {
        }

        public void Reload(Unit unit)
        {
        }

        public void Fire(float unit)
        {
            var now = DateTime.UtcNow;

            if (now - lastFire < TimeSpan.FromMilliseconds(400))
            {
                return;
            }

            lastFire = now;

            magazineCount--;
            Random random = RandomNum();
        
            for (int i = 0; i < 11; i++)
            {
                var newShot = new Shot(this.Engine);
                newShot.Initialize().Attach();
                var offset = (random.NextDouble() - 0.5) * MathHelper.PiOver4;
                var speedChange = random.NextDouble() + 1;
                var direction = (this.body.Rotation + (float)offset).RadiansToDirection();

                newShot.Velocity = direction * this.projectileSpeed * (float)speedChange + this.claimer.LinearVelocity;
                newShot.Rotation = this.body.Rotation;
                newShot.Position = this.body.Position + direction * 2f;
            }


            const float kickback = -25;
            this.Kickbacks.OnNext(this.body.Rotation.RadiansToDirection() * kickback);
        }
        static Random RandomNum()
        {
            Random random = new Random();
            return random;
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateRectangle(this.Engine.World, 1f, 1f, 1f);
            this.body.IsSensor = true;
            this.body.UserData = this;

            disposables.Add(this.body);

            disposables.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.claimer != null)
                                .Subscribe(this.LinkPhysics));

            disposables.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Where(x => this.claimer != null)
                                .Subscribe(this.Update));

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

        private void Update(EngineTime time)
        {
            var direction = this.Engine.Input.GetMouse().KnownWorldPosition - this.Position;
            this.body.Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        private void LinkPhysics(EngineTime time)
        {
            this.body.Position = this.claimer.Position;
        }

        private void Claim(IClaimer owner)
        {
            this.Detach();
            this.claimer = owner;
            this.claimer.Claims.OnNext(this);
        }
    }
}
