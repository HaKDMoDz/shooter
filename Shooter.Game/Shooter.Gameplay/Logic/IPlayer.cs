using System;

namespace Shooter.Gameplay.Logic
{
    public interface IPlayer
    {
        IObservable<IKill> Deaths { get; }
        void Respawn(ISpawnPoint spawnPoint);
    }
}