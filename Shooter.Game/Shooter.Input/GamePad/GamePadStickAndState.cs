using Microsoft.Xna.Framework;

namespace Shooter.Input.GamePad
{
    public class GamePadStickAndState
    {
        public readonly ReactiveGamePad GamePad;
        public readonly GamePadStick Stick;
        public readonly Vector2 Position;

        public GamePadStickAndState(ReactiveGamePad gamepad, GamePadStick stick, Vector2 position)
        {
            this.GamePad = gamepad;
            this.Stick = stick;
            this.Position = position;
        }
    }
}