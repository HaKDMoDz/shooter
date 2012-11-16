using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Input;

namespace Shooter.Gameplay
{
    public class MainMenu : GameObject
    {
        public MainMenu(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Draws.Subscribe(this.Draw));
        }

        public void Draw(EngineTime gameTime)
        {
            this.Engine.Game.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
        }
    }
}
