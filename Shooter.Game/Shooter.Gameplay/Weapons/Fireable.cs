using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using System.Reactive.Subjects;
using Shooter.Gameplay.Claims;

namespace Shooter.Gameplay.Weapons
{
    public static class Fireable
    {
        public static readonly IFireable Empty = new EmptyFireable();

        private class EmptyFireable : IFireable
        {
            private static readonly IObserver<IClaimer> EmptyFireableDependancyProviderObservable =
                   Observer.Create<IClaimer>((x) => { });
            private static readonly IObserver<Unit> EmptyObserver = Observer.Create<Unit>(x => { });
            private static readonly IObserver<float> EmptyFloatObserver = Observer.Create<float>(x => { });
            private static readonly IObservable<Unit> EmptyObservable = Observable.Never<Unit>();
            private static readonly IObservable<Vector2> EmptyVector2Observable = Observable.Never<Vector2>();

            

            public IObserver<float> FireRequests
            {
                get { return EmptyFireable.EmptyFloatObserver; }
            }

            public IObserver<Unit> ReloadRequests
            {
                get { return EmptyFireable.EmptyObserver; }
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

            public IObserver<IClaimer> ClaimRequests
            {
                get { return EmptyFireable.EmptyFireableDependancyProviderObservable; }
            }
        }
    }
}