using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Gameplay.Weapons.Projectiles;
using Shooter.Input.Keyboard;

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
            this.body = BodyFactory.CreateRectangle(this.Engine.World, 1.0f, 1.0f, 0.01f);
            this.body.BodyType = BodyType.Dynamic;
            this.body.Enabled = false;

            this.body.LinearDamping = 2f;
            this.body.AngularDamping = 1f;
            this.body.Restitution = 0.5f;

            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            this.body.Awake = false;

            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            var keyboard = this.Engine.Input.GetKeyboard(PlayerIndex.One);

            attachments.Add(keyboard.PressAsObservable(Keys.OemComma)
                                .Select(x => 1.0f)
                                .Subscribe(this.AngularImpulse));

            attachments.Add(keyboard.PressAsObservable(Keys.OemPeriod)
                                .Select(x => -1.0f)
                                .Subscribe(this.AngularImpulse));

            attachments.Add(this.body.OnCollisionAsObservable().ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => x.FixtureB.Body.UserData is Shot)
                                .Subscribe(this.Explode));
        }

        private void AngularImpulse(float f)
        {
            this.body.AngularVelocity += f * 10;
        }

        private void Explode(CollisionEventArgs args)
        {
            //var explosion = new Explosion(this.Engine.World);

            //explosion.MaxShapes = 10000;

            //explosion.Active(this.Position, 10f, 1000f);

            this.Dispose();
        }
    }
}
