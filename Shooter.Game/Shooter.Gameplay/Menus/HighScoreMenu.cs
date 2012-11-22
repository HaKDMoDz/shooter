using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Input;

namespace Shooter.Gameplay.Menus
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
                this.Engine.Keyboard.PressAsObservable(Keys.Back)
                    .Subscribe(e =>
                                   {
                                       this.Dispose();
                                       new MainMenu(this.Engine).Initialize().Attach();
                                   }));
        }
    }
}