using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Input.Mouse
{
    public class ReactiveMouse
    {
        public IScheduler Scheduler { get; private set; }
        public MouseState State { get; private set; }
        public Point ScreenPosition { get; private set; }
        public Vector2? WorldPosition { get; private set; }
        public Vector2 KnownWorldPosition { get; private set; }

        public IObservable<MouseButtonAndState> Button { get; private set; }
        public Subject<MouseButtonAndState> LeftButton { get; private set; }
        public Subject<MouseButtonAndState> RightButton { get; private set; }
        public Subject<MouseButtonAndState> MiddleButton { get; private set; }
        public Subject<MouseButtonAndState> XButton1 { get; private set; }
        public Subject<MouseButtonAndState> XButton2 { get; private set; }
        public Subject<MouseScroll> Scroll { get; private set; }
        public Subject<MouseMove> Move { get; private set; }

        public ReactiveMouse()
            : this(ReactiveInputManager.DefaultScheduler)
        {
        }

        public ReactiveMouse(IScheduler scheduler)
        {
            this.Scheduler = scheduler;
            this.LeftButton = new Subject<MouseButtonAndState>();
            this.RightButton = new Subject<MouseButtonAndState>();
            this.MiddleButton = new Subject<MouseButtonAndState>();
            this.XButton1 = new Subject<MouseButtonAndState>();
            this.XButton2 = new Subject<MouseButtonAndState>();
            this.Button = Observable.Merge(
                this.LeftButton,
                this.RightButton,
                this.MiddleButton,
                this.XButton1,
                this.XButton2);

            this.Scroll = new Subject<MouseScroll>();

            this.Move = new Subject<MouseMove>();
        }

        public void Update()
        {
            // Copy old state
            var oldState = this.State;
            var oldScreenPosition = this.ScreenPosition;
            var oldWorldPosition = this.WorldPosition;

            // Update to new state
            this.State = Microsoft.Xna.Framework.Input.Mouse.GetState();
            //this.ScreenPosition = this.State.PositionToPoint();
            //this.WorldPosition = this.engine.PerspectiveManager.Unproject(this.State.X, this.State.Y);

            if (this.WorldPosition != null)
            {
                this.KnownWorldPosition = this.WorldPosition.Value;
            }

            if (this.State.LeftButton != oldState.LeftButton)
            {
                this.LeftButton.OnNext(new MouseButtonAndState(this, MouseButton.Left, this.State.LeftButton));
            }

            if (this.State.RightButton != oldState.RightButton)
            {
                this.RightButton.OnNext(new MouseButtonAndState(this, MouseButton.Right, this.State.RightButton));
            }

            if (this.State.MiddleButton != oldState.MiddleButton)
            {
                this.MiddleButton.OnNext(new MouseButtonAndState(this, MouseButton.Middle, this.State.MiddleButton));
            }

            if (this.State.XButton1 != oldState.XButton1)
            {
                this.XButton1.OnNext(new MouseButtonAndState(this, MouseButton.X1, this.State.XButton1));
            }

            if (this.State.XButton2 != oldState.XButton2)
            {
                this.XButton2.OnNext(new MouseButtonAndState(this, MouseButton.X2, this.State.XButton2));
            }

            if (this.State.ScrollWheelValue != 0)
            {
                var scroll = new MouseScroll(this, this.State.ScrollWheelValue);
                this.Scroll.OnNext(scroll);
            }

            if (this.ScreenPosition != oldScreenPosition || this.WorldPosition != oldWorldPosition)
            {
                var screenDelta = new Point(this.State.X - oldState.X, this.State.Y - oldState.Y);

                Vector2? worldDelta = null;

                if (this.WorldPosition != null && oldWorldPosition != null)
                {
                    worldDelta = this.WorldPosition - oldWorldPosition;
                }

                var move = new MouseMove(this, this.ScreenPosition, screenDelta, this.WorldPosition, worldDelta);

                this.Move.OnNext(move);
            }
        }
    }
}
