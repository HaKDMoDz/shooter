using System;
using System.Reactive.Subjects;

namespace Microsoft.Xna.Framework.Input.Reactive.GamePad
{
    public class ReactiveGamePadDPad : IObservable<GamePadButtonAndState>
    {
        private readonly ReactiveGamePad gamepad;
        private readonly Subject<GamePadButtonAndState> buttons = new Subject<GamePadButtonAndState>();

        public ReactiveGamePadDPad(ReactiveGamePad gamepad)
        {
            this.gamepad = gamepad;

            this.Up = new ReactiveGamePadButton(this, GamePadButton.Up);
            this.Down = new ReactiveGamePadButton(this, GamePadButton.Down);
            this.Left = new ReactiveGamePadButton(this, GamePadButton.Left);
            this.Right = new ReactiveGamePadButton(this, GamePadButton.Right);
        }


        public ReactiveGamePadButton Up { get; private set; }
        public ReactiveGamePadButton Down { get; private set; }
        public ReactiveGamePadButton Left { get; private set; }
        public ReactiveGamePadButton Right { get; private set; }

        internal void Update()
        {
            this.NextIfChanged(GamePadButton.Up, x => x.DPad.Up);
            this.NextIfChanged(GamePadButton.Down, x => x.DPad.Down);
            this.NextIfChanged(GamePadButton.Left, x => x.DPad.Left);
            this.NextIfChanged(GamePadButton.Right, x => x.DPad.Right);
        }

        public IDisposable Subscribe(IObserver<GamePadButtonAndState> observer)
        {
            return this.buttons.Subscribe(observer);
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