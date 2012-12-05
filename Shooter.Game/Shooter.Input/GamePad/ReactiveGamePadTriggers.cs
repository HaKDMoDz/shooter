using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Shooter.Input.GamePad
{
    public class ReactiveGamePadTriggers : IObservable<GamePadTriggerAndState>
    {
        private readonly ReactiveGamePad gamepad;
        private readonly Subject<GamePadTriggerAndState> triggers = new Subject<GamePadTriggerAndState>();

        public ReactiveGamePadTriggers(ReactiveGamePad gamepad)
        {
            this.gamepad = gamepad;

            this.Left = this.triggers.Where(x => x.Trigger == GamePadTrigger.Left);
            this.Right = this.triggers.Where(x => x.Trigger == GamePadTrigger.Right);
        }

        public IObservable<GamePadTriggerAndState> Left { get; private set; }
        public IObservable<GamePadTriggerAndState> Right { get; private set; }

        internal void Update()
        {
            if (Math.Abs(this.gamepad.State.Triggers.Left - this.gamepad.OldState.Triggers.Left) > 0)
            {
                this.triggers.OnNext(new GamePadTriggerAndState(this.gamepad, GamePadTrigger.Left, this.gamepad.State.Triggers.Left));
            }

            if (Math.Abs(this.gamepad.State.Triggers.Right - this.gamepad.OldState.Triggers.Right) > 0)
            {
                this.triggers.OnNext(new GamePadTriggerAndState(this.gamepad, GamePadTrigger.Right, this.gamepad.State.Triggers.Right));
            }
        }

        public IDisposable Subscribe(IObserver<GamePadTriggerAndState> observer)
        {
            return this.triggers.Subscribe(observer);
        }
    }
}