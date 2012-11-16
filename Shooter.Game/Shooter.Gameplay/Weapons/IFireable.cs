using System.Reactive;
using System;

namespace Shooter.Gameplay.Weapons
{
    public interface IFireable
    {
        IObserver<Unit> FireRequests { get; }
        IObserver<Unit> ReloadRequests { get; }
    }
}