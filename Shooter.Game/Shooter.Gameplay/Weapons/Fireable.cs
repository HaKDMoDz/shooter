using System;
using System.Reactive;
using System.Reactive.Subjects;
namespace Shooter.Gameplay.Weapons
{
    public class Fireable
    {
        public static readonly IFireable Empty = new EmptyFireable();

        private class EmptyFireable : IFireable
        {
            private Subject<Unit> fireRequests = new Subject<Unit>();
            private Subject<Unit> reloadRequests = new Subject<Unit>();

            public IObserver<Unit> FireRequests { get { return this.fireRequests; } }
            public IObserver<Unit> ReloadRequests { get { return this.reloadRequests; } }
        }
    }
}