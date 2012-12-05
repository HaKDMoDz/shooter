using Microsoft.Xna.Framework.Input;

namespace Shooter.Input.Keyboard
{
    public struct KeyAndState
    {
        public readonly ReactiveKeyboard Keyboard;
        public readonly Keys Key;
        public readonly KeyState State;

        public KeyAndState(ReactiveKeyboard keyboard, Keys key, KeyState state)
        {
            this.Keyboard = keyboard;
            this.Key = key;
            this.State = state;
        }
    }
}