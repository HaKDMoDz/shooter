using System;
using System.Reactive;
using Microsoft.Xna.Framework;

namespace Shooter.Gameplay.Weapons
{
    public interface IFireable
    {
        IObserver<float> FireRequests { get; }
        IObserver<Unit> ReloadRequests { get; }

        IObservable<Unit> Fires { get; }
        IObservable<Unit> Reloads { get; }
        IObservable<Vector2> Kickbacks { get; }
    }
}