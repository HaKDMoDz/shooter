// -----------------------------------------------------------------------
// <copyright file="Camera.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shooter.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Camera
    {
        private float zoom;

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public float VerticalUnits { get; set; }
        public float Zoom
        {
            get { return zoom; }
            set { this.zoom = MathHelper.Clamp(value, 0.25f, 1f); }
        }

        public float ZoomFactor
        {
            get { return 1f / (float)Math.Pow(this.zoom, 3); }
        }

        public Camera()
            : this(10.0f)
        {
        }

        public Camera(float verticalUnits)
        {
            this.VerticalUnits = verticalUnits;
            this.Zoom = 1f;
        }

        /// <summary>
        /// Retrieves the World View Projection matrix for a UISpriteBatch the combination of this camera and the viewport.
        /// </summary>
        /// <returns></returns>
        public Matrix GetMatrix(Viewport viewport)
        {
            var aspectRatio = viewport.Width / (float)viewport.Height;

            var viewMatrix = Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0f) *
                             Matrix.CreateRotationZ(this.Rotation);

            var verticalUnits = this.VerticalUnits * this.ZoomFactor;

            var projectionMatrix = Matrix.CreateOrthographic(aspectRatio * verticalUnits, verticalUnits, 0, 1f);

            var matrix = viewMatrix * projectionMatrix;

            return matrix;
        }
    }
}