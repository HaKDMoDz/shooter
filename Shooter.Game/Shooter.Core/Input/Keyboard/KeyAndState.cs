using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Input.Keyboard
{
    public struct KeyAndState
    {
        public Keys Key;
        public KeyState State;

        public KeyAndState(Keys key, KeyState state)
        {
            this.Key = key;
            this.State = state;
        }
    }
}