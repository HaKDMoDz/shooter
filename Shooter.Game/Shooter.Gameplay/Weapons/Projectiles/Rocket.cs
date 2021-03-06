﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using FarseerPhysics.Common.PhysicsLogic;

namespace Shooter.Gameplay.Weapons.Projectiles
{
    public class Rocket : GameObject
    {
        private Body body;

        public Rocket(Engine engine)
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
            this.body = BodyFactory.CreateRectangle(this.Engine.World, 1.0f, 0.25f, 2f);

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
            attachments.Add(Disposable.Create(() => this.body.Enabled = false));
            attachments.Add(this.body.OnCollisionAsObservable()
                                .Where(x => !x.FixtureB.IsSensor)
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Subscribe(this.OnCollision));
        }

        private void OnCollision(CollisionEventArgs args)
        {
            var explosion = new Explosion(this.Engine.World);

            explosion.MaxShapes = 10000;

            explosion.Activate(this.Position, 10f, 1000f);

            this.Dispose();
        }
    }
}
