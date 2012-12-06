using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
    public class Flamethrower : Weapon
    {
        private Body body;
        private float projectileSpeed = 10f;
        private IClaimer claimer;

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        public Flamethrower(Engine engine)
            : base(engine)
        {
        }

        public static readonly Random Random = new Random();
        private Texture2D texture;

        public void Fire(IFireRequest request)
        {
            for (int i = 0; i < 1; i++)
            {
                var newFlame = new Flame(this.Engine, request);
                newFlame.Initialize().Attach();
                var direction = (this.body.Rotation + MathHelper.Pi * (float)(Random.NextDouble() - .5) / 4).RadiansToDirection();

                newFlame.Velocity = direction * this.projectileSpeed * (float)(Random.NextDouble() + 1) + this.claimer.LinearVelocity;
                newFlame.Rotation = this.body.Rotation;
                newFlame.Position = this.body.Position;
            }
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Weapons/FlameThrower");

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

            disposables.Add(
                this.FireRequests.Take(1)
                    .Concat(
                        Observable.Interval(TimeSpan.FromMilliseconds(10)).Take(1)
                            .Where(x => false)
                            .Select<long, IFireRequest>(x => null))
                    .Repeat()
                    .ObserveOn(this.Engine.PostPhysicsScheduler)
                    .Subscribe(this.Fire));
        }

        public void Draw(EngineTime engineTime)
        {
            this.Engine.SpriteBatch.Draw(this.texture, this.Position, null, Color.White, MathHelper.Pi,
                                         this.texture.Size() / 2,
                                         Vector2.One / this.texture.Size(), SpriteEffects.None, 0f);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.ClaimRequests
                                .Where(x => this.claimer == null)
                                .Subscribe(this.Claim));

            attachments.Add(this.Engine.PerspectiveDraws.Subscribe(this.Draw));
        }

        private void Update(EngineTime time)
        {
            var direction = this.Engine.Input.GetMouse().KnownWorldPosition - this.Position;
            this.body.Rotation = this.claimer.Rotation;
        }

        private void LinkPhysics(EngineTime time)
        {
            this.body.Position = this.claimer.Position;
        }

        private void Claim(IClaimer claimer)
        {
            this.Detach();
            this.claimer = claimer;
            this.claimer.Claims.OnNext(this);
        }
    }
}
