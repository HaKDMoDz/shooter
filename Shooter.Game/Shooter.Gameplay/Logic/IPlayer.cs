using System;
using System.Reactive.Subjects;
using Shooter.Gameplay.Menus.Models;

namespace Shooter.Gameplay.Logic
{
    public interface IPlayer
    {
        string Name { get; }
        PlayerTeam Team { get; }
        IObservable<IKill> Deaths { get; }

        void Spawn(ISpawnPoint spawnPoint);
    }
}