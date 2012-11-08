using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Xna.Extensions
{
    public static class MouseStateExtensions
    {
        public static Vector2 PositionToVector2(this MouseState state)
        {
            return new Vector2(state.X, state.Y);
        }

        public static Point PositionToPoint(this MouseState state)
        {
            return new Point(state.X, state.Y);
        }
    }
}