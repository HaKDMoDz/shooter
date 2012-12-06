using System.Collections.Generic;

namespace Shooter.Gameplay.Logic
{
    public interface ISpawnPointProvider
    {
        ISpawnPoint GetSpawnPoint(IPlayer player);
    }
}