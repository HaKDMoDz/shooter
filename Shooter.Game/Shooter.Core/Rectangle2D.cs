using Microsoft.Xna.Framework;

namespace Shooter.Core
{
    public struct Rectangle2D
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Rectangle2D(float left, float right, float top, float bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }

        public Rectangle2D(Vector2 min, Vector2 max)
        {
            this.Left = min.X;
            this.Right = max.X;
            this.Top = max.Y;
            this.Bottom = min.Y;
        }

        public bool Intersects(Rectangle2D other)
        {
            return Rectangle2D.Intersects(ref this, ref other);
        }

        public static bool Intersects(ref Rectangle2D a, ref Rectangle2D b)
        {
            if (a.Right < b.Left ||
                a.Left > b.Right ||
                a.Top < b.Bottom ||
                a.Bottom > b.Top)
                return false;

            return true;
        }
    }
}