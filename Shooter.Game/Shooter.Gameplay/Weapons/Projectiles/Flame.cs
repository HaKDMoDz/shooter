using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Core.Xna.Extensions;
using Shooter.Gameplay.Logic;

namespace Shooter.Gameplay.Weapons.Projectiles
{
    public class Flame : GameObject, IContactDamager, IPlayerFire
    {
        private readonly IFireRequest request;
        private Body body;

        private DateTime timeToDelete;
        private Texture2D texture;

        public Flame(Engine engine, IFireRequest request)
            : base(engine)
        {
            this.request = request;
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        public Vector2 Velocity
        {
            get { return this.body.LinearVelocity; }
            set { this.body.LinearVelocity = value; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
            set { this.body.Rotation = value; }
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Weapons/Flame");

            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.01f, 0.01f, float.Epsilon);

            this.body.BodyType = BodyType.Dynamic;
            this.body.IsBullet = true;
            this.body.UserData = this;
            this.body.Enabled = false;
            this.body.CollisionCategories = Category.Cat31;
            this.body.CollidesWith = Category.All ^ Category.Cat31;
            this.body.IgnoreCollisionWith(this.request.Body);
            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            var birth = DateTime.UtcNow;
            this.timeToDelete = birth + TimeSpan.FromMilliseconds(1000);

            attachments.Add(Disposable.Create(() => this.body.Enabled = false));
            attachments.Add(this.body.OnCollisionAsObservable()
                                .Where(x => !x.FixtureB.IsSensor)
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Subscribe(this.OnCollision));

            attachments.Add(this.Engine.Updates.ObserveOn(this.Engine.UpdateScheduler)
                                .Subscribe(this.CheckLifetime));

            attachments.Add(this.Engine.PerspectiveDraws.Subscribe(this.Draw));
        }

        private void Draw(EngineTime engineTime)
        {
            this.Engine.SpriteBatch.Draw(this.texture, this.Position, null, Color.White, MathHelper.Pi,
                                         this.texture.Size() / 2,
                                         Vector2.One / this.texture.Size() * 0.5f, SpriteEffects.None, 0f);
        }

        private void OnCollision(CollisionEventArgs args)
        {
            this.Dispose();
        }

        private void CheckLifetime(EngineTime time)
        {
            var now = DateTime.UtcNow;

            if (now > this.timeToDelete)
            {
                this.Dispose();
            }
        }

        IPlayer IContactDamager.Player
        {
            get { return this.request.Player; }
        }

        public IContactDamage GetContactDamage(IContactDamagable damagable)
        {
            return new ContactDamage(5.0f);
        }

        IPlayer IPlayerFire.Player
        {
            get { return this.request.Player; }
        }
    }
}