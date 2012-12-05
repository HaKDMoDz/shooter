using System;
using System.Reactive.Linq;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Input.GamePad
{
    public class ReactiveGamePadButton : IObservable<GamePadButtonAndState>
    {
        private readonly IObservable<GamePadButtonAndState> button;

        public ReactiveGamePadButton(IObservable<GamePadButtonAndState> buttons, GamePadButton button)
        {
            this.button = buttons.Where(x => x.Button == button);

            this.Press = this.button.Where(x => x.State == ButtonState.Pressed);
            this.Release = this.button.SkipUntil(this.Press).Where(x => x.State == ButtonState.Released);
        }

        public IObservable<GamePadButtonAndState> Press { get; private set; }
        public IObservable<GamePadButtonAndState> Release { get; private set; }

        public IDisposable Subscribe(IObserver<GamePadButtonAndState> observer)
        {
            return this.button.Subscribe(observer);
        }
    }
}