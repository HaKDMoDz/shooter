using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Input.Keyboard;

namespace Shooter.Gameplay.Menus.Views
{
    public class HighScoreMenu : GameObject
    {
        public HighScoreMenu(Engine engine)
            : base(engine)
        {
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(
                this.Engine.Input.GetKeyboard(PlayerIndex.One).PressAsObservable(Keys.Back)
                    .Subscribe(e =>
                                   {
                                       this.Dispose();
                                       new MainMenu(this.Engine).Initialize().Attach();
                                   }));
        }
    }
}