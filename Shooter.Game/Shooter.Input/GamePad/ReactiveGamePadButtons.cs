using System;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Input.GamePad
{
    public class ReactiveGamePadButtons : IObservable<GamePadButtonAndState>
    {
        private readonly ReactiveGamePad gamepad;
        private readonly Subject<GamePadButtonAndState> buttons = new Subject<GamePadButtonAndState>();

        public ReactiveGamePadButton A { get; private set; }
        public ReactiveGamePadButton B { get; private set; }
        public ReactiveGamePadButton X { get; private set; }
        public ReactiveGamePadButton Y { get; private set; }

        public ReactiveGamePadButton LeftShoulder { get; private set; }
        public ReactiveGamePadButton RightShoulder { get; private set; }

        public ReactiveGamePadButton LeftStick { get; private set; }
        public ReactiveGamePadButton RightStick { get; private set; }

        public ReactiveGamePadButton Back { get; private set; }
        public ReactiveGamePadButton Start { get; private set; }
        public ReactiveGamePadButton BigButton { get; private set; }

        internal ReactiveGamePadButtons(ReactiveGamePad gamepad)
        {
            this.gamepad = gamepad;

            this.A = new ReactiveGamePadButton(this, GamePadButton.A);
            this.B = new ReactiveGamePadButton(this, GamePadButton.B);
            this.X = new ReactiveGamePadButton(this, GamePadButton.X);
            this.Y = new ReactiveGamePadButton(this, GamePadButton.Y);

            this.LeftShoulder = new ReactiveGamePadButton(this, GamePadButton.LeftShoulder);
            this.RightShoulder = new ReactiveGamePadButton(this, GamePadButton.RightShoulder);

            this.LeftStick = new ReactiveGamePadButton(this, GamePadButton.LeftStick);
            this.RightStick = new ReactiveGamePadButton(this, GamePadButton.RightStick);

            this.Back = new ReactiveGamePadButton(this, GamePadButton.Back);
            this.Start = new ReactiveGamePadButton(this, GamePadButton.Start);
            this.BigButton = new ReactiveGamePadButton(this, GamePadButton.BigButton);
        }


        public IDisposable Subscribe(IObserver<GamePadButtonAndState> observer)
        {
            return this.buttons.Subscribe(observer);
        }

        internal void Update()
        {
            // Letters
            this.NextIfChanged(GamePadButton.A, x => x.Buttons.A);
            this.NextIfChanged(GamePadButton.B, x => x.Buttons.B);
            this.NextIfChanged(GamePadButton.X, x => x.Buttons.X);
            this.NextIfChanged(GamePadButton.Y, x => x.Buttons.Y);

            // Shoulders
            this.NextIfChanged(GamePadButton.LeftShoulder, x => x.Buttons.LeftShoulder);
            this.NextIfChanged(GamePadButton.RightShoulder, x => x.Buttons.RightShoulder);

            // Sticks
            this.NextIfChanged(GamePadButton.LeftStick, x => x.Buttons.LeftStick);
            this.NextIfChanged(GamePadButton.RightStick, x => x.Buttons.RightStick);

            // Other
            this.NextIfChanged(GamePadButton.Back, x => x.Buttons.Back);
            this.NextIfChanged(GamePadButton.Start, x => x.Buttons.Start);
            this.NextIfChanged(GamePadButton.BigButton, x => x.Buttons.BigButton);
        }

        private void NextIfChanged(GamePadButton button, Func<GamePadState, ButtonState> selector)
        {
            var temp = selector(this.gamepad.State);

            if (temp != selector(this.gamepad.OldState))
            {
                this.buttons.OnNext(new GamePadButtonAndState(this.gamepad, button, temp));
            }
        }
    }
}