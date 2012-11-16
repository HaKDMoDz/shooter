using System;
using System.Reactive;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Shooter.Core;

namespace Shooter.Gameplay.Weapons
{
    public class Weapon : GameObject, IFireable
    {
        public Weapon(Engine engine) : base(engine)
        {
            this.FireRequests   = new Subject<Unit>();
            this.ReloadRequests = new Subject<Unit>();
            this.Fires          = new Subject<Unit>();
            this.Reloads        = new Subject<Unit>();
            this.Kickbacks      = new Subject<Vector2>();
        }

        protected Subject<Unit> FireRequests { get; set; }
        protected Subject<Unit> ReloadRequests { get; set; }
        protected Subject<Unit> Fires { get; set; }
        protected Subject<Unit> Reloads { get; set; }
        protected Subject<Vector2> Kickbacks { get; set; }

        IObservable<Vector2> IFireable.Kickbacks
        {
            get { return this.Kickbacks; }
        }

        IObserver<Unit> IFireable.FireRequests
        {
            get { return this.FireRequests; }
        }

        IObserver<Unit> IFireable.ReloadRequests
        {
            get { return this.ReloadRequests; }
        }

        IObservable<Unit> IFireable.Fires
        {
            get { return this.Fires; }
        }

        IObservable<Unit> IFireable.Reloads
        {
            get { return this.Reloads; }
        }
    }
}