using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Input.Keyboard
{
    public static class ReactiveKeyboardExtensions
    {
        public static IObservable<KeyAndState> PressAsObservable(this ReactiveKeyboard keyboard, Keys key)
        {
            return keyboard.KeyStateDictionary[key].Skip(1).Where(x => x.State == KeyState.Down);
        }

        public static IObservable<KeyAndState> ReleaseAsObservable(this ReactiveKeyboard keyboard, Keys key)
        {
            return keyboard.KeyStateDictionary[key].Skip(1).Where(x => x.State == KeyState.Up);
        }

        public static IObservable<KeyAndState> LongPressAsObservable(this ReactiveKeyboard keyboard, Keys key)
        {
            return LongPressAsObservable(
                keyboard.PressAsObservable(key),
                keyboard.ReleaseAsObservable(key),
                keyboard.Scheduler
                );
        }

        public static IObservable<KeyAndState> LongPressAsObservable(this ReactiveKeyboard keyboard, Keys key, TimeSpan delay)
        {
            return LongPressAsObservable(
                keyboard.PressAsObservable(key),
                keyboard.ReleaseAsObservable(key),
                delay,
                keyboard.Scheduler
                );
        }

        private static IObservable<KeyAndState> LongPressAsObservable(IObservable<KeyAndState> press, IObservable<KeyAndState> release, IScheduler scheduler)
        {
            return LongPressAsObservable(press, release, TimeSpan.FromSeconds(0.5), scheduler);
        }

        private static IObservable<KeyAndState> LongPressAsObservable(IObservable<KeyAndState> press, IObservable<KeyAndState> release, TimeSpan delay, IScheduler scheduler)
        {
            return press.SelectMany(
                pressed =>
                Observable
                    .Return(pressed)
                    .Delay(delay, scheduler)
                    .TakeUntil(release.Where(released => pressed.Key == released.Key)));
        }
    }
}