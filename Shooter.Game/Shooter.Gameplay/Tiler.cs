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

        public Tiler(Engine engine)
            : base(engine)
        {
            this.texture = engine.Game.Content.Load<Texture2D>("BackgroundTile");
            this.texture = engine.Game.Content.Load<Texture2D>("Textures/Floor");
        }

        protected override void OnInitialize(System.Collections.Generic.ICollection<IDisposable> disposables)
        {
        }

        protected override void OnAttach(System.Collections.Generic.ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.PerspectiveDraws.Subscribe(this.Draw));
        }

        public void Draw(EngineTime time)
        {
            const int width = 20;
            const int height = 20;

            var scale = Vector2.One * 4;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var position = new Vector2(x - width / 2, y - height / 2) * scale;
                    this.Engine.SpriteBatch.Draw(this.texture, position, null, Color.White,
                                                 0f,
                                                 this.texture.Size() / 2, Vector2.One / this.texture.Size() * scale,
                                                 SpriteEffects.None, 0f);
                }
            }


            //var perspective = this.Engine.PerspectiveManager.CurrentPerspective;

            //var bounds = perspective.GetBounds(this.Engine.PerspectiveManager.Bounds);

            //var xmin = (int)Math.Floor(bounds.Left);
            //var xmax = (int)Math.Ceiling(bounds.Right + 1);
            //var ymin = (int)Math.Floor(bounds.Bottom);
            //var ymax = (int)Math.Ceiling(bounds.Top + 1);

            //var scale = 1;

            //while (Math.Abs(xmax - xmin) / scale > 10 || Math.Abs(ymax - ymin) / scale > 10)
            //{
            //    scale *= 10;
            //}

            //while (xmin % scale != 0)
            //{
            //    xmin--;
            //    xmax++;
            //}

            //while (ymin % scale != 0)
            //{
            //    ymin--;
            //    ymax++;
            //}

            //for (var x = xmin; x <= xmax; x += scale)
            //{
            //    for (var y = ymin; y <= ymax; y += scale)
            //    {
            //        var position = new Vector2(x, y);

            //        this.Engine.SpriteBatch.Draw(
            //            this.texture,
            //            position,
            //            null,
            //            Color.White,
            //            0f,
            //            Vector2.Zero,
            //            Vector2.One / this.texture.Size() * scale,
            //            SpriteEffects.None,
            //            -1f
            //            );
            //    }
            //}
        }
    }
}