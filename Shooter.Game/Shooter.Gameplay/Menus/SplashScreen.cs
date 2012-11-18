using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core;
using Shooter.Core.Xna.Extensions;

namespace Shooter.Gameplay.Menus
{
    public class SplashScreen : GameObject
    {
        private Texture2D splashBackground;

        public SplashScreen(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.splashBackground = this.Engine.Game.Content.Load<Texture2D>("Menu/SplashImage");
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Draws.ObserveOn(this.Engine.PostDrawScheduler).Subscribe(this.Draw));
            attachments.Add(
                Observable.Interval(TimeSpan.FromSeconds(2.5)).Take(1)
                    .Select(x => Unit.Default)
                    .Subscribe(this.ShowMainMenu));
        }

        private void Draw(EngineTime time)
        {
            var width = this.Engine.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var height = this.Engine.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            var offset = this.splashBackground.Size() / 2;

            var position = new Vector2(width, height) / 2 - offset;

            this.Engine.UISpriteBatch.Draw(this.splashBackground, position, Color.White);
        }

        private void ShowMainMenu(Unit unit)
        {
            this.Detach();
            new MainMenu(this.Engine).Initialize().Attach();
        }
    }
}