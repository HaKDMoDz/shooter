﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Core.Input;
using Shooter.Gameplay.Weapons.Projectiles;

namespace Shooter.Gameplay.Prefabs
{
    public class FlyingBrick : GameObject
    {
        private Body body;

        public FlyingBrick(Engine engine)
            : base(engine)
        {
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateRectangle(this.Engine.World, 1f, 1.618f, 0.01f);
            this.body.BodyType = BodyType.Dynamic;
            this.body.Enabled = false;

            this.body.LinearDamping = 1f;
            this.body.AngularDamping = 1f;

            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            this.body.Awake = false;

            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.Engine.Keyboard.PressAsObservable(Keys.OemComma)
                                .Select(x => 1.0f)
                                .Subscribe(this.AngularImpulse));

            attachments.Add(this.Engine.Keyboard.PressAsObservable(Keys.OemPeriod)
                                .Select(x => -1.0f)
                                .Subscribe(this.AngularImpulse));

            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Where(x => x.FixtureB.Body.UserData is Bolt)
                                .Subscribe(this.Explode));
        }

        private void AngularImpulse(float f)
        {
            this.body.AngularVelocity += f * 10;
        }

        private void Explode(CollisionEventArgs args)
        {
            var explosion = new FarseerPhysics.Common.PhysicsLogic.Explosion(this.Engine.World);

            explosion.Activate(this.Position, 3f, 1000f);

            this.Dispose();
        }
    }
}
