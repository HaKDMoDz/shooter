using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Core
{
    public class ViewportProvider : IViewportProvider
    {
        private readonly Func<Rectangle, Viewport> viewportFactory;

        public ViewportProvider(Func<Rectangle, Viewport> viewportFactory)
        {
            this.viewportFactory = viewportFactory;
        }

        public Viewport GetViewport(Rectangle rectangle)
        {
            return this.viewportFactory(rectangle);
        }
    }
}