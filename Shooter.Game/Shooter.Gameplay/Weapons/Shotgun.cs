using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private float projectileSpeed = 25f;
        private IClaimer claimer;
        public Vector2 Position { get { return this.body.Position; } set { this.body.Position = value; } }
        private static readonly Random Random = new Random();

        private DateTime lastFire = DateTime.MinValue;
        private Texture2D texture;

        public Shotgun(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Weapons/Shotgun");

            this.body = BodyFactory.CreateRectangle(this.Engine.World, 0.5f, 0.5f, 1f);
            this.body.Enabled = false;
            this.body.IsSensor = true;
            this.body.UserData = this;

            disposables.Add(this.body);

            disposables.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.claimer != null)
                                .Subscribe(this.LinkPhysics));

            disposables.Add(
                this.FireRequests.Take(1)
                    .Concat(
                        Observable.Interval(TimeSpan.FromMilliseconds(1000)).Take(1)
                            .Where(x => false)
                            .Select<long, IFireRequest>(x => null))
                    .Repeat()
                    .ObserveOn(this.Engine.PostPhysicsScheduler)
                    .Subscribe(this.Fire));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.claimer = null;
            this.body.Enabled = true;

            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.ClaimRequests
                                .Where(x => this.claimer == null)
                                .Subscribe(this.Claim));

            attachments.Add(this.Engine.PerspectiveDraws.Subscribe(this.Draw));
        }

        private void Draw(EngineTime engineTime)
        {
            this.Engine.SpriteBatch.Draw(this.texture, this.Position, null, Color.White, MathHelper.Pi,
                                         this.texture.Size() / 2,
                                         Vector2.One / this.texture.Size(), SpriteEffects.None, 0f);
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

        private void Fire(IFireRequest request)
        {
            for (int i = 0; i <= 10; i++)
            {
                var newShot = new Shot(this.Engine, request);
                newShot.Initialize().Attach();
                var offset = (Random.NextDouble() - 0.5) * MathHelper.PiOver4;
                var speedChange = Random.NextDouble() + 1;
                var direction = (this.body.Rotation + (float) offset).RadiansToDirection();

                newShot.Velocity = direction * this.projectileSpeed * (float)speedChange + this.claimer.LinearVelocity;
                newShot.Rotation = this.body.Rotation;
                newShot.Position = this.body.Position;
            }

            const float kickback = -25;
            this.Kickbacks.OnNext(this.body.Rotation.RadiansToDirection() * kickback);
        }
    }
}
