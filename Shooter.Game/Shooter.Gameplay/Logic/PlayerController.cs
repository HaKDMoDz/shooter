using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Input.GamePad;
using Shooter.Input.Keyboard;

namespace Shooter.Gameplay.Logic
{
    public class PlayerController : GameObject
    {
        private readonly Player player;
        private readonly ReactiveGamePad gamepad;
        private ReactiveKeyboard keyboard;

        public PlayerController(Engine engine, Player player, PlayerIndex playerIndex):base(engine)
        {
            this.player = player;
            this.gamepad = this.Engine.Input.GetGamePad(playerIndex);
            this.keyboard = this.Engine.Input.GetKeyboard(playerIndex);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            // Movement
            attachments.Add(this.gamepad.ThumbSticks.Left
                                .Select(x => x.Position)
                                .Subscribe(this.player.SetMovement));

            attachments.Add(this.keyboard.KeyStates.Where(x => x.Key == Keys.W ||
                                                               x.Key == Keys.A ||
                                                               x.Key == Keys.S ||
                                                               x.Key == Keys.D)
                                .Select(x =>
                                            {
                                                var movement = Vector2.Zero;

                                                if (x.Keyboard.State.IsKeyDown(Keys.W))
                                                {
                                                    movement += Vector2.UnitY;
                                                }

                                                if (x.Keyboard.State.IsKeyDown(Keys.S))
                                                {
                                                    movement -= Vector2.UnitY;
                                                }

                                                if (x.Keyboard.State.IsKeyDown(Keys.A))
                                                {
                                                    movement -= Vector2.UnitX;
                                                }

                                                if (x.Keyboard.State.IsKeyDown(Keys.D))
                                                {
                                                    movement += Vector2.UnitX;
                                                }
                                                if (movement.LengthSquared() > 0)
                                                {
                                                    movement.Normalize();
                                                }

                                                return movement;

                                            }).Subscribe(this.player.SetMovement));

            // Turret
            attachments.Add(this.gamepad.ThumbSticks.Right
                                .Where(x => x.Position.LengthSquared() > 0)
                                .Select(x => (float) Math.Atan2(x.Position.Y, x.Position.X))
                                .Subscribe(this.player.SetTurretRotation));

            // Fire (Constant fire, once per update, when the right trigger is pulled at least 50%
            attachments.Add(this.gamepad.Triggers.Right
                                .Where(x => x.Amount >= 0.5f)
                                .SelectMany(x => this.Engine.Updates.Select(y => x.Amount)
                                                     .TakeUntil(this.gamepad.Triggers.Right))
                                .Subscribe(this.player.Fire));
        }
    }
}
