using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Input.GamePad
{
    public struct GamePadButtonAndState
    {
        public readonly ReactiveGamePad GamePad;
        public readonly GamePadButton Button;
        public readonly ButtonState State;

        public GamePadButtonAndState(ReactiveGamePad gamePad, GamePadButton button, ButtonState state)
        {
            this.GamePad = gamePad;
            this.Button = button;
            this.State = state;
        }
    }
}