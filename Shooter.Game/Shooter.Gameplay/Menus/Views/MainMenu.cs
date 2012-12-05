using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Gameplay.Menus.Models;
using Shooter.Input.Keyboard;

namespace Shooter.Gameplay.Menus.Views
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

            this.button1.Action = () =>
                                 {
                                     this.Dispose();
                                     new PreGameController(this.Engine).Initialize().Attach();
                                 };

            this.button2.Action = () =>
                                 {
                                     this.Dispose();
                                     new HighScoreMenu(this.Engine).Initialize().Attach();
                                 };

            this.buttons.Add(this.button1);
            this.buttons.Add(this.button2);

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

            var keyboard = this.Engine.Input.GetKeyboard(PlayerIndex.One);
            var gamepad = this.Engine.Input.GetGamePad(PlayerIndex.One);

            attachments.Add(
                Observable.Merge(
                    keyboard.PressAsObservable(Keys.Up).Select(x => Unit.Default),
                    gamepad.DPad.Up.Press.Select(x => Unit.Default))
                    .Subscribe(this.SelectPrevButton));

            attachments.Add(
                Observable.Merge(
                    keyboard.PressAsObservable(Keys.Down).Select(x => Unit.Default),
                    gamepad.DPad.Down.Press.Select(x => Unit.Default))
                    .Subscribe(this.SelectNextButton));
            attachments.Add(
                Observable.Merge(
                    keyboard.PressAsObservable(Keys.Enter).Select(x => Unit.Default),
                    gamepad.Buttons.A.Press.Select(x => Unit.Default))
                    .Subscribe(e => this.currentButton.First().Action()));
        }

        private void SelectPrevButton(Unit unit)
        {
            var button = this.currentButton.First();

            var index = this.buttons.IndexOf(button);

            if(--index < 0)
            {
                index = this.buttons.Count - 1;
            }

            this.oldButton.OnNext(button);
            this.currentButton.OnNext(this.buttons[index]);
        }

        private void SelectNextButton(Unit unit)
        {
            var button = this.currentButton.First();

            var index = this.buttons.IndexOf(button);

            if (++index >= this.buttons.Count)
            {
                index = 0;
            }

            this.oldButton.OnNext(button);
            this.currentButton.OnNext(this.buttons[index]);
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