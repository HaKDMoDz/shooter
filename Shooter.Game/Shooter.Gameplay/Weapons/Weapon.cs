using System;
using System.Reactive;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Gameplay.Claims;
using Shooter.Gameplay.Logic;

namespace Shooter.Gameplay.Weapons
{
    public class Weapon : GameObject, IFireable, IClaimable
    {

        public Weapon(Engine engine) : base(engine)
        {
            this.FireRequests = new Subject<IFireRequest>();
            this.ReloadRequests = new Subject<Unit>();
            this.Fires          = new Subject<Unit>();
            this.Reloads        = new Subject<Unit>();
            this.Kickbacks      = new Subject<Vector2>();
            this.ClaimRequests  = new Subject<IClaimer>();
        }

        protected Subject<IFireRequest> FireRequests { get; private set; }
        protected Subject<Unit> ReloadRequests { get; private set; }
        protected Subject<Unit> Fires { get; private set; }
        protected Subject<Unit> Reloads { get; private set; }
        protected Subject<Vector2> Kickbacks { get; private set; }
        protected Subject<IClaimer> ClaimRequests { get; private set; }

        IObserver<IClaimer> IClaimable.ClaimRequests
        {
            get { return this.ClaimRequests; }
        }

        IObservable<Vector2> IFireable.Kickbacks
        {
            get { return this.Kickbacks; }
        }

        IObserver<IFireRequest> IFireable.FireRequests
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

    public interface IFireRequest
    {
        IPlayer Player { get; }
        float Amount { get; }
    }
}