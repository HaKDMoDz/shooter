using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Reactive.Subjects;


namespace Shooter.Core.Input
{
    public class ReactiveGamePad
    {
        private readonly PlayerIndex playerIndex;

        public GamePadState State { get; private set; }

        public Subject<GamePadButtonAndState> Up { get; private set; }
        public Subject<GamePadButtonAndState> Down { get; private set; }
        public Subject<GamePadButtonAndState> Left { get; private set; }
        public Subject<GamePadButtonAndState> Right { get; private set; }

        public Subject<GamePadButtonAndState> Back { get; private set; }
        public Subject<GamePadButtonAndState> Start { get; private set; }

        public ReactiveGamePadButtons Buttons { get; private set; }
        public ReactiveGamePadButtons DPad { get; private set; }

        public ReactiveGamePad(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;

            this.Buttons = new ReactiveGamePadButtons(this);
        }

        public void Update()
        {
            var oldState = this.State;
            this.State = GamePad.GetState(this.playerIndex);
            this.Buttons.Update(this.State, oldState);
        }
    }
}
