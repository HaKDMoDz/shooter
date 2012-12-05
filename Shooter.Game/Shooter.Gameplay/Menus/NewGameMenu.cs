using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Input;
using Shooter.Core.Input.Keyboard;

namespace Shooter.Gameplay.Menus
{
    public class NewGameMenu : GameObject
    {
        public NewGameMenu(Engine engine)
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

            attachments.Add(
                this.Engine.Keyboard.PressAsObservable(Keys.Enter)
                    .Subscribe(e =>
                                   {
                                       var camera = new Camera();

                                       this.Engine.PerspectiveManager.Perspectives.Add(
                                           new Perspective(camera, new Viewport(0, 0, 100, 100)
                                               ));

                                       this.Dispose();
                                       new Levels.SampleLevel(this.Engine).Initialize().Attach();
                                   }));
        }
    }
}