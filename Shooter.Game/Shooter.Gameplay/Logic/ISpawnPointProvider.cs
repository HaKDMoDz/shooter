using System.Collections.Generic;

namespace Shooter.Gameplay.Logic
{
    public interface ISpawnPointProvider
    {
        IEnumerable<ISpawnPoint> GetSpawnPoints();
    }
}