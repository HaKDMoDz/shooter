using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core.Xna.Extensions;

namespace Shooter.Core
{
    public class Perspective
    {
        public Viewport Viewport { get; private set; }
        public Camera Camera { get; private set; }

        public Perspective(Camera camera, Viewport viewport)
        {
            this.Camera = camera;
            this.Viewport = viewport;
        }

        public Matrix GetMatrix()
        {
            return this.Camera.GetMatrix(this.Viewport);
        }

        public Rectangle2D GetBounds()
        {
            float ymin = 0;
            float xmin = 0;
            float xmax = this.Viewport.Width;
            float ymax = this.Viewport.Height;

            var matrix = Matrix.Invert(this.GetMatrix() * SpriteBatchExtensions.GetUndoMatrix(this.Viewport));

            var corners = new[]
                {
                    new Vector2(xmin, ymin),
                    new Vector2(xmax, ymin),
                    new Vector2(xmin, ymax),
                    new Vector2(xmax, ymax),
                };

            var vectors = corners.Select(x => Vector2.Transform(x, matrix)).ToList();

            xmin = vectors.Min(x => x.X);
            xmax = vectors.Max(x => x.X);
            ymin = vectors.Min(x => x.Y);
            ymax = vectors.Max(x => x.Y);

            return new Rectangle2D(xmin, xmax, ymax, ymin);
        }
    }
}