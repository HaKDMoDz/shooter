using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Input
{
    public class ReactiveGamePadButtons
    {
        private ReactiveGamePad reactiveGamePad;

        public Subject<GamePadButtonAndState> A { get; private set; }
        public Subject<GamePadButtonAndState> B { get; private set; }
        public Subject<GamePadButtonAndState> X { get; private set; }
        public Subject<GamePadButtonAndState> Y { get; private set; }

        public Subject<GamePadButtonAndState> LeftShoulder { get; private set; }
        public Subject<GamePadButtonAndState> RightShoulder { get; private set; }

        public Subject<GamePadButtonAndState> LeftStick { get; private set; }
        public Subject<GamePadButtonAndState> RightStick { get; private set; }

        public ReactiveGamePadButtons(ReactiveGamePad reactiveGamePad)
        {
            this.reactiveGamePad = reactiveGamePad;

            this.A = new Subject<GamePadButtonAndState>();
            this.B = new Subject<GamePadButtonAndState>();
            this.X = new Subject<GamePadButtonAndState>();
            this.Y = new Subject<GamePadButtonAndState>();

            this.LeftShoulder = new Subject<GamePadButtonAndState>();
            this.RightShoulder = new Subject<GamePadButtonAndState>();

            this.LeftStick = new Subject<GamePadButtonAndState>();
            this.RightStick = new Subject<GamePadButtonAndState>();
        }

        public void Update(GamePadState state, GamePadState oldState)
        {
            if (state.Buttons.A != oldState.Buttons.A)
            {
                this.A.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.A, state.Buttons.A));
            }

            if (state.Buttons.B != oldState.Buttons.B)
            {
                this.B.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.B, state.Buttons.B));
            }

            if (state.Buttons.X != oldState.Buttons.X)
            {
                this.X.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.X, state.Buttons.X));
            }

            if (state.Buttons.Y != oldState.Buttons.Y)
            {
                this.Y.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.Y, state.Buttons.Y));
            }

            if (state.Buttons.LeftShoulder != oldState.Buttons.LeftShoulder)
            {
                this.LeftShoulder.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.LeftShoulder, state.Buttons.LeftShoulder));
            }

            if (state.Buttons.RightShoulder != oldState.Buttons.RightShoulder)
            {
                this.RightShoulder.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.RightShoulder, state.Buttons.RightShoulder));
            }

            if (state.Buttons.LeftStick != oldState.Buttons.LeftStick)
            {
                this.LeftStick.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.LeftStick, state.Buttons.LeftStick));
            }

            if (state.Buttons.RightStick != oldState.Buttons.RightStick)
            {
                this.RightStick.OnNext(new GamePadButtonAndState(this.reactiveGamePad, GamePadButton.RightStick, state.Buttons.RightStick));
            }
        }
    }
}
