using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Shooter.Input.GamePad
{
    public class ReactiveGamePadThumbSticks : IObservable<GamePadStickAndState>
    {
        private readonly ReactiveGamePad gamepad;
        private readonly Subject<GamePadStickAndState> sticks = new Subject<GamePadStickAndState>();

        public ReactiveGamePadThumbSticks(ReactiveGamePad gamepad)
        {
            this.gamepad = gamepad;

            this.sticks = new Subject<GamePadStickAndState>();

            this.Left = this.sticks.Where(x => x.Stick == GamePadStick.Left);
            this.Right = this.sticks.Where(x => x.Stick == GamePadStick.Right);
        }

        public IObservable<GamePadStickAndState> Right { get; private set; }
        public IObservable<GamePadStickAndState> Left { get; private set; }

        internal void Update()
        {
            if (this.gamepad.State.ThumbSticks.Left != this.gamepad.OldState.ThumbSticks.Left)
            {
                this.sticks.OnNext(new GamePadStickAndState(this.gamepad, GamePadStick.Left, this.gamepad.State.ThumbSticks.Left));
            }

            if (this.gamepad.State.ThumbSticks.Right != this.gamepad.OldState.ThumbSticks.Right)
            {
                this.sticks.OnNext(new GamePadStickAndState(this.gamepad, GamePadStick.Right, this.gamepad.State.ThumbSticks.Right));
            }
        }

        public IDisposable Subscribe(IObserver<GamePadStickAndState> observer)
        {
            return this.sticks.Subscribe(observer);
        }
    }
}