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
using Shooter.Core.Input;
using Shooter.Core.Xna.Extensions;
using Shooter.Gameplay.Powerups;
using Shooter.Gameplay.Weapons;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Gameplay
{
    public class Robot : GameObject
    {
        private Body body;
        private Texture2D texture;
        private IFireable weapon;
        //private SpriteBatch spriteBatch = new SpriteBatch();
        private readonly List<IPowerup> powerups = new List<IPowerup>();
        private const float MaxLinearAcceleration = 75f;

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

        public void Draw()
        {
            
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.25f, 1);
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
                                .Subscribe(this.UpdateInput));

            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.Engine.Mouse.State.LeftButton == ButtonState.Pressed)
                                .Subscribe(this.Fire));

            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.Engine.GamePad.State.Buttons.A == ButtonState.Pressed)
                                .Subscribe(this.Fire));

            attachments.Add(this.Engine.Keyboard.PressAsObservable(Keys.R)
                                .ObserveOn(this.Engine.InputScheduler)
                                .Subscribe(this.Reload));

            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Select(x => x.FixtureB.Body.UserData)
                                .OfType<IFireable>()
                                .Subscribe(this.SetFireable));

            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Select(x => x.FixtureB.Body.UserData as IPowerup)
                                .Where(x => x != null)
                                .Subscribe(this.CollectPowerup));

            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Subscribe(this.LinkPhysics));

            attachments.Add(this.Engine.PerspectiveDraws.Subscribe(this.Draw));
        }

        private void Draw(EngineTime engineTime)
        {
            var texture = this.Engine.Game.Content.Load<Texture2D>("BackgroundTile"); // Dont do this, Andrew. Its bad.
            var size = 2f;
            this.Engine.SpriteBatch.Draw(texture, this.body.Position, null, Color.Orange, 0, texture.Size() / 2,
                                         Vector2.One / texture.Size() * size, SpriteEffects.None, 0f);
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

            direction += Vector2.UnitX * this.Engine.GamePad.State.ThumbSticks.Left.X;
            direction += Vector2.UnitY * this.Engine.GamePad.State.ThumbSticks.Left.Y;

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            var attributes = new RobotTraits();

            powerups.RemoveAll((x) => x.ShouldRemove);

            foreach (var powerup in powerups)
            {
                powerup.Process(attributes);
            }

            direction *= attributes.AccelerationRate;

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
            this.weapon.Kickbacks.Subscribe(x => this.body.LinearVelocity += x);
        }

        private void Fire(EngineTime time)
        {
            this.weapon.FireRequests.OnNext(Unit.Default);
        }

        private void Reload(KeyAndState key)
        {
            this.weapon.ReloadRequests.OnNext(Unit.Default);
        }
    }
}