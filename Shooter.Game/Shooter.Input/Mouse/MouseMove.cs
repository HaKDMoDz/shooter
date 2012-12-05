using Microsoft.Xna.Framework;

namespace Shooter.Input.Mouse
{
    public class MouseMove
    {
        public readonly ReactiveMouse Mouse;
        public readonly Point ScreenPosition;
        public readonly Point ScreenDelta;
        public readonly Vector2? WorldPosition;
        public readonly Vector2? WorldDelta;

        public MouseMove(ReactiveMouse mouse, Point screenPosition, Point screenDelta, Vector2? worldPosition, Vector2? worldDelta)
        {
            this.Mouse = mouse;
            this.ScreenPosition = screenPosition;
            this.ScreenDelta = screenDelta;
            this.WorldPosition = worldPosition;
            this.WorldDelta = worldDelta;
        }
    }
}