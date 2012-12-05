using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Input.Mouse;

namespace Shooter.Input.Keyboard
{
    public class ReactiveKeyboard
    {
        private readonly PlayerIndex playerIndex;
        private readonly IObservable<KeyAndState> keyState;

        internal readonly Dictionary<Keys, BehaviorSubject<KeyAndState>> KeyStateDictionary;

        public IScheduler Scheduler { get; private set; }
        public KeyboardState State { get; private set; }

        public IObservable<KeyAndState> KeyStates
        {
            get { return this.keyState; }
        }

        public ReactiveKeyboard(PlayerIndex playerIndex)
            : this(playerIndex, ReactiveInputManager.DefaultScheduler)
        {
        }

        public ReactiveKeyboard(PlayerIndex playerIndex, IScheduler scheduler)
        {
            this.playerIndex = playerIndex;

            this.Scheduler = scheduler;

            this.KeyStateDictionary = Enum
                .GetValues(typeof (Keys))
                .Cast<Keys>()
                .ToDictionary(
                    (x) => x,
                    (x) => new BehaviorSubject<KeyAndState>(
                               new KeyAndState(this, x, KeyState.Up)
                               )
                );

            this.keyState = this.KeyStateDictionary.Values.ToObservable().Merge();
        }

        public void Update()
        {
            this.State = Microsoft.Xna.Framework.Input.Keyboard.GetState(this.playerIndex);

            var all = this.KeyStateDictionary
                .Select(x => x.Value.First())
                .ToList();

            var previouslyDown = all
                .Where(x => x.State == KeyState.Down)
                .Select(x => x.Key);

            var previouslyUp = all
                .Where(x => x.State == KeyState.Up)
                .Select(x => x.Key);

            var currentlyDown = this.State
                .GetPressedKeys();

            var currentlyUp = all
                .Select(x => x.Key)
                .Except(currentlyDown);

            var justPressed = currentlyDown.Except(previouslyDown);

            var justReleased = currentlyUp.Except(previouslyUp);

            foreach(var key in justPressed)
            {
                this.KeyStateDictionary[key].OnNext(new KeyAndState(this, key, KeyState.Down));
            }

            foreach(var key in justReleased)
            {
                this.KeyStateDictionary[key].OnNext(new KeyAndState(this, key, KeyState.Up));
            }
        }
    }
}
