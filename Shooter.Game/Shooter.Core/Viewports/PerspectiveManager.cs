using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Core
{
    public class PerspectiveManager : IEnumerable<IPerspective>
    {
        private readonly Engine engine;
        public List<IPerspective> Perspectives { get; private set; }

        public IPerspective CurrentPerspective { get; set; }

        public Rectangle Bounds { get; private set; }

        public PerspectiveManager(Engine engine)
        {
            this.engine = engine;
            this.Perspectives = new List<IPerspective>();
            this.Bounds = this.engine.Game.Window.ClientBounds;

            // BUG: This is a memory leak ->
            this.engine.Game.GraphicsDevice.DeviceReset += (a, b) => this.Bounds = this.engine.Game.Window.ClientBounds;
        }

        public IEnumerator<IPerspective> GetEnumerator()
        {
            return this.Perspectives.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Vector2? Unproject(int x, int y)
        {
            foreach (var perspective in this.Perspectives.Where(p => p.GetViewport(this.Bounds).Bounds.Contains(x, y)).Take(1))
            {
                //return Vector2.Transform(
                //    new Vector2(x, y),
                //    Matrix.Invert(
                //        perspective.GetMatrix()
                //        )
                //    );

                var v = new Vector3(x, y, 0);

                v = perspective.GetViewport(this.Bounds).Unproject(v, perspective.GetMatrix(this.Bounds), Matrix.Identity, Matrix.Identity);

                return new Vector2(v.X, v.Y);
            }

            return null;
        }

        public Vector2? Unproject(Point screenPosition)
        {
            return this.Unproject(screenPosition.X, screenPosition.Y);
        }

        public Perspective CreatePerspective(Camera camera, Func<Rectangle, Viewport> viewportFactory)
        {
            var perspecetive = new Perspective(this, camera, viewportFactory);
            this.Perspectives.Add(perspecetive);
            return perspecetive;
        }
    }
}