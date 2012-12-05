using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Input.Mouse
{
    public class MouseButtonAndState
    {
        public readonly ReactiveMouse Mouse;
        public readonly ButtonState State;
        public readonly MouseButton Button;

        public MouseButtonAndState(ReactiveMouse mouse, MouseButton button, ButtonState state)
        {
            this.Mouse = mouse;
            this.Button = button;
            this.State = state;
        }
    }
}