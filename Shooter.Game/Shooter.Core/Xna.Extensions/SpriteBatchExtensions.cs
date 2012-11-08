using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Core.Xna.Extensions
{
    public class SpriteBatchExtensions
    {
        public static Matrix GetUndoMatrix(Viewport viewport)
        {
            return Matrix.CreateScale(0.5f, -0.5f, 0.5f) *
                   Matrix.CreateTranslation(0.5f, 0.5f, 0.0f) *
                   Matrix.CreateScale(viewport.Width, viewport.Height, 1f);
        }
    }
}