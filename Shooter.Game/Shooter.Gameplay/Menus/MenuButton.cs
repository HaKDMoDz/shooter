using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core;
using Shooter.Core.Xna.Extensions;

namespace Shooter.Gameplay.Menus
{
    public class MenuButton : GameObject
    {
        private Texture2D buttonTexture;
        private SpriteFont font;

        public MenuButton(Engine engine) : base(engine)
        {
            this.Text = "";
            this.Color = Color.Black;
        }

        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.font = this.Engine.Game.Content.Load<SpriteFont>("Menu/Font");
            this.buttonTexture = this.Engine.Game.Content.Load<Texture2D>("Menu/Button");
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Draws.ObserveOn(this.Engine.PostDrawScheduler).Subscribe(this.Draw));
        }

        private void Draw(EngineTime time)
        {
            var width = this.Engine.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = this.Engine.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            var offset = this.buttonTexture.Size() / 2;

            var screenSize = new Vector2(width, height);

            var textSize = font.MeasureString(this.Text);
            var textPosition = this.Position - textSize / 2;

            this.Engine.UISpriteBatch.Draw(this.buttonTexture, this.Position - offset + screenSize / 2, Color.White);
            this.Engine.UISpriteBatch.DrawString(this.font, this.Text,
                                                 Vector2.UnitY * 2 + textPosition + screenSize / 2, this.Color);
        }
    }
}
