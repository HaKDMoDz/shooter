using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Input;

namespace Shooter.Gameplay.Menus
{
    public class MainMenu : GameObject
    {
        private readonly MenuButton button1;
        private readonly MenuButton button2;

        private readonly List<MenuButton> buttons = new List<MenuButton>(); 

        private readonly BehaviorSubject<MenuButton> currentButton;
        private readonly BehaviorSubject<MenuButton> oldButton;

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

            button1.Action = () =>
                                 {
                                     this.Dispose();
                                     new NewGameMenu(this.Engine).Initialize().Attach();
                                 };

            button2.Action = () =>
                                 {
                                     this.Dispose();
                                     new HighScoreMenu(this.Engine).Initialize().Attach();
                                 };

            this.buttons.Add(button1);
            this.buttons.Add(button2);

            this.oldButton = new BehaviorSubject<MenuButton>(null);
            this.currentButton = new BehaviorSubject<MenuButton>(this.button1);
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.button1.Initialize();
            this.button2.Initialize();

            disposables.Add(Disposable.Create(() => this.button1.Dispose()));
            disposables.Add(Disposable.Create(() => this.button2.Dispose()));
            disposables.Add(this.oldButton.Where(x => x != null).Subscribe(this.OnButtonDeselected));
            disposables.Add(this.currentButton.Subscribe(this.OnButtonSelected));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.button1.Attach();
            this.button2.Attach();

            attachments.Add(Disposable.Create(() => this.button1.Detach()));
            attachments.Add(Disposable.Create(() => this.button2.Detach()));

            attachments.Add(this.Engine.Keyboard.PressAsObservable(Keys.Up).Subscribe(this.SelectPrevButton));
            attachments.Add(this.Engine.Keyboard.PressAsObservable(Keys.Down).Subscribe(this.SelectNextButton));
            attachments.Add(
                this.Engine.Keyboard.PressAsObservable(Keys.Enter).Delay(TimeSpan.FromMilliseconds(1)).Subscribe(
                    e => this.currentButton.First().Action()));
        }

        private void SelectPrevButton(KeyAndState kas)
        {
            var button = this.currentButton.First();

            var index = buttons.IndexOf(button);

            if(--index < 0)
            {
                index = buttons.Count - 1;
            }

            this.oldButton.OnNext(button);
            this.currentButton.OnNext(buttons[index]);
        }

        private void SelectNextButton(KeyAndState kas)
        {
            var button = this.currentButton.First();

            var index = buttons.IndexOf(button);

            if (++index >= buttons.Count)
            {
                index = 0;
            }

            this.oldButton.OnNext(button);
            this.currentButton.OnNext(buttons[index]);
        }

        private void OnButtonDeselected(MenuButton button)
        {
            button.Color = Color.Black;
        }

        private void OnButtonSelected(MenuButton button)
        {
            button.Color = Color.Red;
        }
    }
}