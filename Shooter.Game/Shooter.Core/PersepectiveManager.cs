using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Shooter.Core
{
    public class PerspectiveManager : IEnumerable<Perspective>
    {
        public List<Perspective> Perspectives { get; private set; }

        public Perspective CurrentPerspective { get; set; }

        public PerspectiveManager()
        {
            this.Perspectives = new List<Perspective>();
        }

        public IEnumerator<Perspective> GetEnumerator()
        {
            return this.Perspectives.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Vector2? Unproject(int x, int y)
        {
            foreach (var perspective in this.Perspectives.Where(p => p.Viewport.Bounds.Contains(x, y)).Take(1))
            {
                //return Vector2.Transform(
                //    new Vector2(x, y),
                //    Matrix.Invert(
                //        perspective.GetMatrix()
                //        )
                //    );

                var v = new Vector3(x, y, 0);

                v = perspective.Viewport.Unproject(v, perspective.GetMatrix(), Matrix.Identity, Matrix.Identity);

                return new Vector2(v.X, v.Y);
            }

            return null;
        }

        public Vector2? Unproject(Point screenPosition)
        {
            return this.Unproject(screenPosition.X, screenPosition.Y);
        }
    }
}