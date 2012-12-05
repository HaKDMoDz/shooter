namespace Shooter.Core.Input
{
    public class MouseScroll
    {
        public readonly ReactiveMouse Mouse;
        public readonly int Delta;

        public MouseScroll(ReactiveMouse mouse, int delta)
        {
            this.Mouse = mouse;
            this.Delta = delta;
        }
    }
}