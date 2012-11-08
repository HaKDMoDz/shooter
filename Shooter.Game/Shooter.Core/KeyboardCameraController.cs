using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core.Input;

namespace Shooter.Core
{
    public class KeyboardCameraController : GameObject
    {
        private readonly Camera camera;

        public float MoveSpeed { get; set; }
        public float ZoomSpeed { get; set; }

        public KeyboardCameraController(Engine engine, Camera camera)
            : base(engine)
        {
            this.camera = camera;

            this.MoveSpeed = 5f;
            this.ZoomSpeed = 0.5f;
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Updates.ObserveOn(this.Engine.UpdateScheduler).Subscribe(this.UpdateInput));
            attachments.Add(this.Engine.Keyboard.PressAsObservable(Keys.R).Subscribe(this.Beacon));
        }

        private void Beacon(KeyAndState key)
        {
            var position = this.camera.Position;

            Observable
                .Return(0, this.Engine.InputScheduler).Delay(TimeSpan.FromSeconds(10))
                .Subscribe(
                    (x) => this.camera.Position = position
                );
        }

        /// <summary>
        /// Takes input for the camera
        /// </summary>
        /// <param name="time"></param>
        private void UpdateInput(EngineTime time)
        {
            Vector2 linearVelocity = Vector2.Zero;

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.Up))
            {
                linearVelocity += Vector2.UnitY;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.Down))
            {
                linearVelocity -= Vector2.UnitY;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.Left))
            {
                linearVelocity -= Vector2.UnitX;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.Right))
            {
                linearVelocity += Vector2.UnitX;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.Z))
            {
                this.camera.Zoom += this.ZoomSpeed*time.Elapsed;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.X))
            {
                this.camera.Zoom -= this.ZoomSpeed*time.Elapsed;
            }

            if (this.Engine.Keyboard.State.IsKeyDown(Keys.C))
            {
                this.Dispose();
            }

            this.camera.Position += linearVelocity*this.MoveSpeed*this.camera.ZoomFactor*time.Elapsed;
        }
    }
}