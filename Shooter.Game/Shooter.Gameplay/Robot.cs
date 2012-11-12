using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Core.Input;
using Shooter.Gameplay.Powerups;
using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay
{
    public class Robot : GameObject
    {
        private Body body;
        private IFireable weapon;
        private readonly List<IPowerup> powerups = new List<IPowerup>();
        private const float MaxLinearAcceleration = 250f;

        public Robot(Engine engine)
            : base(engine)
        {
            this.weapon = Fireable.Empty;
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
            set { this.body.Rotation = value; }
        }

        public Vector2 LinearVelocity
        {
            get { return this.body.LinearVelocity; }
            set { this.body.LinearVelocity = value; }
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 1f, 1);
            this.body.BodyType = BodyType.Dynamic;
            this.body.LinearDamping = 10f;
            this.body.UserData = this;
            this.body.FixedRotation = true;

            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.InputScheduler)
                                .Subscribe(this.UpdateInput));

            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.InputScheduler)
                                .Where(x => this.Engine.Mouse.State.LeftButton == ButtonState.Pressed)
                                .Subscribe(this.Fire));

            attachments.Add(this.Engine.Keyboard.PressAsObservable(Keys.R)
                                .ObserveOn(this.Engine.InputScheduler)
                                .Subscribe(this.Reload));

            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => x.FixtureB.Body.UserData is IFireable)
                                .Select(x => (IFireable) x.FixtureB.Body.UserData)
                                .Subscribe(this.SetFireable));

            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Select(x => x.FixtureB.Body.UserData as IPowerup)
                                .Where(x => x != null)
                                .Subscribe(this.CollectPowerup));

            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Subscribe(this.LinkPhysics));
        }

        private void CollectPowerup(IPowerup powerup)
        {
            this.powerups.Add(powerup);
        }

        private void UpdateInput(EngineTime time)
        {
            var direction = Vector2.Zero;

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.A))
            {
                direction -= Vector2.UnitX;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.D))
            {
                direction += Vector2.UnitX;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.W))
            {
                direction += Vector2.UnitY;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.S))
            {
                direction -= Vector2.UnitY;
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            var attributes = new RobotAttributes();

            powerups.RemoveAll((x) => x.ShouldRemove);

            foreach (var powerup in powerups)
            {
                powerup.Process(attributes);
            }

            direction *= attributes.AccelerationFactor;

            this.body.LinearVelocity += direction * MaxLinearAcceleration * time.Elapsed;
        }

        private void LinkPhysics(EngineTime time)
        {
            if (this.body.LinearVelocity.LengthSquared() > 0.5)
            {
                this.Rotation = (float) Math.Atan2(this.body.LinearVelocity.Y, this.body.LinearVelocity.X);
            }
        }

        private void SetFireable(IFireable fireable)
        {
            this.weapon = fireable;
        }

        private void Fire(EngineTime time)
        {
            this.weapon.Fire();
        }

        private void Reload(KeyAndState key)
        {
            this.weapon.Reload();
        }
    }
}