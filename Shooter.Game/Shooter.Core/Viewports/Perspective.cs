using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core.Xna.Extensions;

namespace Shooter.Core
{
    public class Perspective : IPerspective, IDisposable
    {
        private readonly PerspectiveManager manager;
        private readonly IViewportProvider viewportProvider;

        public Camera Camera { get; private set; }

        public Perspective(PerspectiveManager manager, Camera camera, Func<Rectangle, Viewport> viewportFactory)
            : this(manager, camera, new ViewportProvider(viewportFactory))
        {
        }

        public Perspective(PerspectiveManager manager, Camera camera, IViewportProvider viewportProvider)
        {
            this.manager = manager;
            this.Camera = camera;
            this.viewportProvider = viewportProvider;
        }

        public Matrix GetMatrix(Rectangle rectangle)
        {
            return this.Camera.GetMatrix(this.viewportProvider.GetViewport(rectangle));
        }

        public Rectangle2D GetBounds(Rectangle rectangle)
        {
            float ymin = 0;
            float xmin = 0;
            float xmax = 0;
            float ymax = 0;

            var matrix =
                Matrix.Invert(this.GetMatrix(rectangle) *
                              SpriteBatchExtensions.GetUndoMatrix(this.viewportProvider.GetViewport(rectangle)));

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

        public Viewport GetViewport(Rectangle rectangle)
        {
            return this.viewportProvider.GetViewport(rectangle);
        }

        public void Dispose()
        {
            this.manager.Perspectives.Remove(this);
        }
    }
}