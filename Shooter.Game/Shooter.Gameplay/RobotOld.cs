using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Gameplay.Logic;
using Shooter.Gameplay.Powerups;
using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay
{
    public class RobotOld : GameObject
    {
        private Body body;
        private IFireable weapon;
        private readonly List<IPowerup> powerups = new List<IPowerup>();
        private const float MaxLinearAcceleration = 200f;

        public RobotOld(Engine engine)
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
            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.5f, 1);
            this.body.BodyType = BodyType.Dynamic;
            this.body.LinearDamping = 5f;
            this.body.UserData = this;
            this.body.FixedRotation = true;

            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.InputScheduler)
                                .Subscribe(this.Update));

            //attachments.Add(this.Engine.Updates
            //                    .ObserveOn(this.Engine.PostPhysicsScheduler)
            //                    .Where(x => this.Engine.Input.GetMouse().State.LeftButton == ButtonState.Pressed)
            //                    .Subscribe(this.Fire));

            var collisions = this.body.OnCollisionAsObservable()
                .ObserveOn(this.Engine.PostPhysicsScheduler)
                .Select(x => x.FixtureB.Body.UserData);

            //attachments.Add(this.Engine.Updates
            //                    .ObserveOn(this.Engine.PostPhysicsScheduler)
            //                    .Where(x => this.Engine.GamePad.State.Buttons.A == ButtonState.Pressed)
            //                    .Subscribe(this.Fire));

            attachments.Add(collisions.OfType<IFireable>().Subscribe(this.CollectWeapon));
            attachments.Add(collisions.OfType<IPowerup>().Subscribe(this.CollectPowerup));

            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Subscribe(this.LinkPhysics));
        }

        private void CollectPowerup(IPowerup powerup)
        {
            this.powerups.Add(powerup);
        }

        private void Update(EngineTime time)
        {
            //var direction = Vector2.Zero;

            //var keyboard = this.Engine.Input.GetKeyboard(PlayerIndex.One);

            //if (keyboard.State.IsKeyDown(Keys.A))
            //{
            //    direction -= Vector2.UnitX;
            //}

            //if (keyboard.State.IsKeyDown(Keys.D))
            //{
            //    direction += Vector2.UnitX;
            //}

            //if (keyboard.State.IsKeyDown(Keys.W))
            //{
            //    direction += Vector2.UnitY;
            //}

            //if (keyboard.State.IsKeyDown(Keys.S))
            //{
            //    direction -= Vector2.UnitY;
            //}

            ////direction += Vector2.UnitX * this.Engine.GamePad.State.ThumbSticks.Previous.X;
            ////direction += Vector2.UnitY * this.Engine.GamePad.State.ThumbSticks.Previous.Y;

            //if (direction != Vector2.Zero)
            //{
            //    direction.Normalize();
            //}

            //var attributes = new RobotTraits();

            //powerups.RemoveAll((x) => x.ShouldRemove);

            //foreach (var powerup in powerups)
            //{
            //    powerup.Process(attributes);
            //}

            //direction *= attributes.AccelerationRate;

            //this.body.LinearVelocity += direction * MaxLinearAcceleration * time.Elapsed;
        }

        private void LinkPhysics(EngineTime time)
        {
            if (this.body.LinearVelocity.LengthSquared() > 0.5)
            {
                this.Rotation = (float) Math.Atan2(this.body.LinearVelocity.Y, this.body.LinearVelocity.X);
            }
        }

        private void Fire(EngineTime time)
        {
            this.weapon.FireRequests.OnNext(Unit.Default);
        }

        private void CollectWeapon(IFireable fireable)
        {
            this.weapon = fireable;
            this.weapon.Kickbacks.Subscribe(x => this.body.LinearVelocity += x);
        }
    }
}