using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;

namespace Shooter.Gameplay.Weapons.Projectiles
{
    public class Flame : GameObject
    {
        private Body body;

        private DateTime timeToDelete;

        public Flame(Engine engine)
            : base(engine)
        {
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
            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.01f, 0.01f, float.Epsilon);

            this.body.BodyType = BodyType.Dynamic;
            this.body.IsBullet = true;
            this.body.UserData = this;
            this.body.Enabled = false;
            this.body.CollisionCategories = Category.Cat31;
            this.body.CollidesWith = Category.All ^ Category.Cat31;
            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            var birth = DateTime.UtcNow;
            this.timeToDelete = birth + TimeSpan.FromMilliseconds(100);



            attachments.Add(Disposable.Create(() => this.body.Enabled = false));
            attachments.Add(this.body.OnCollisionAsObservable()
                                .Where(x => !x.FixtureB.IsSensor)
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Subscribe(this.OnCollision));

            attachments.Add(this.Engine.Updates.ObserveOn(this.Engine.UpdateScheduler)
                                .Subscribe(this.CheckLifetime));
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
    }
}