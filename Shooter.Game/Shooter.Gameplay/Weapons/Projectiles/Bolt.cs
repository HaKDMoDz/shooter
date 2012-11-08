using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;

namespace Shooter.Gameplay.Weapons.Projectiles
{
    public class Bolt : GameObject
    {
        private Body body;

        public Bolt(Engine engine)
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
            this.body = BodyFactory.CreateRectangle(this.Engine.World, 0.5f, 0.1f, 2f);
            this.body.BodyType = BodyType.Dynamic;
            this.body.IsBullet = true;
            this.body.UserData = this;
            const Category collisionCatagory = Category.Cat31;
            this.body.CollisionCategories = collisionCatagory;
            this.body.CollidesWith = Category.All ^ collisionCatagory;
            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Subscribe(this.OnCollision));
        }

        private void OnCollision(CollisionEventArgs args)
        {
            this.Dispose();
        }
    }
}
