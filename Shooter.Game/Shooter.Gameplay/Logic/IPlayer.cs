using System;
using System.Reactive.Subjects;

namespace Shooter.Gameplay.Logic
{
    public interface IPlayer
    {
        IObservable<IKill> Deaths { get; }
        void Respawn(ISpawnPoint spawnPoint);
    }
}