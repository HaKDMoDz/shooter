using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;

namespace Shooter.Gameplay.Weapons
{
    public static class Fireable
    {
        public static readonly IFireable Empty = new EmptyFireable();

        private class EmptyFireable : IFireable
        {
            private static readonly IObserver<Unit> EmtpyObserver = Observer.Create<Unit>(x => { });
            private static readonly IObservable<Unit> EmptyObservable = Observable.Never<Unit>();
            private static readonly IObservable<Vector2> EmptyVector2Observable = Observable.Never<Vector2>();

            public IObserver<Unit> FireRequests
            {
                get { return EmptyFireable.EmtpyObserver; }
            }

            public IObserver<Unit> ReloadRequests
            {
                get { return EmptyFireable.EmtpyObserver; }
            }

            public IObservable<Unit> Fires
            {
                get { return EmptyFireable.EmptyObservable; }
            }

            public IObservable<Unit> Reloads
            {
                get { return EmptyFireable.EmptyObservable; }
            }

            public IObservable<Vector2> Kickbacks
            {
                get { return EmptyFireable.EmptyVector2Observable; }
            }
        }
    }
}