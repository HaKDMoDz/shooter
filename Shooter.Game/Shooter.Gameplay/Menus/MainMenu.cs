using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Shooter.Core;

namespace Shooter.Gameplay.Menus
{
    public class MainMenu : GameObject
    {
        private readonly MenuButton button1;
        private readonly MenuButton button2;

        public MainMenu(Engine engine)
            : base(engine)
        {
            this.button1 = new MenuButton(this.Engine);
            this.button2 = new MenuButton(this.Engine);

            this.button1.Position = new Vector2(0, -40);
            this.button2.Position = new Vector2(0, 40);

            this.button1.Text = "new game";
            this.button2.Text = "high scores";

            this.button1.Color = new Color(0.3f, 0.3f, 0.3f);
            this.button2.Color = new Color(0.3f, 0.3f, 0.3f);
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.button1.Initialize();
            this.button2.Initialize();

            disposables.Add(Disposable.Create(() => this.button1.Dispose()));
            disposables.Add(Disposable.Create(() => this.button2.Dispose()));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Draws.ObserveOn(this.Engine.PostDrawScheduler).Subscribe(this.Draw));

            this.button1.Attach();
            this.button2.Attach();

            attachments.Add(Disposable.Create(() => this.button1.Detach()));
            attachments.Add(Disposable.Create(() => this.button2.Detach()));
        }

        private void Draw(EngineTime time)
        {
        }
    }
}