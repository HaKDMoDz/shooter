using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core;
using Shooter.Core.Xna.Extensions;

namespace Shooter.Gameplay
{
    public class Tiler : GameObject
    {
        private readonly Texture2D texture;
        private SpriteBatch spriteBatch;

        public Tiler(Engine engine)
            : base(engine)
        {
            this.texture = engine.Game.Content.Load<Texture2D>("BackgroundTile");
        }

        protected override void OnInitialize(System.Collections.Generic.ICollection<IDisposable> disposables)
        {
            disposables.Add(this.spriteBatch = new SpriteBatch(this.Engine.Game.GraphicsDevice));
        }

        protected override void OnAttach(System.Collections.Generic.ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.PerspectiveDraws.Subscribe(this.Draw));
        }

        public void Draw(EngineTime time)
        {
            var perspective = this.Engine.PerspectiveManager.CurrentPerspective;

            var bounds = perspective.GetBounds(this.Engine.PerspectiveManager.Bounds);

            var xmin = (int)Math.Floor(bounds.Left);
            var xmax = (int)Math.Ceiling(bounds.Right + 1);
            var ymin = (int)Math.Floor(bounds.Bottom);
            var ymax = (int)Math.Ceiling(bounds.Top + 1);

            var scale = 1;

            while (Math.Abs(xmax - xmin) / scale > 10 || Math.Abs(ymax - ymin) / scale > 10)
            {
                scale *= 10;
            }

            while (xmin % scale != 0)
            {
                xmin--;
                xmax++;
            }

            while (ymin % scale != 0)
            {
                ymin--;
                ymax++;
            }


            var sbMatrix = perspective.GetMatrix(this.Engine.PerspectiveManager.Bounds) * SpriteBatchExtensions.GetUndoMatrix(perspective.GetViewport(this.Engine.PerspectiveManager.Bounds));

            this.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                null,
                RasterizerState.CullNone,
                null,
                sbMatrix
                );

            for (var x = xmin; x <= xmax; x += scale)
            {
                for (var y = ymin; y <= ymax; y += scale)
                {
                    var position = new Vector2(x, y);

                    this.spriteBatch.Draw(
                        this.texture,
                        position,
                        null,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        Vector2.One / this.texture.Size() * scale,
                        SpriteEffects.None,
                        -1f
                        );
                }
            }

            this.spriteBatch.End();
        }
    }
}