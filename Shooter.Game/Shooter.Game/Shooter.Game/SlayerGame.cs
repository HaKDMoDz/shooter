using System;
using System.Collections.Generic;
using System.Linq;
using Shooter.Gameplay.Logic;

namespace Shooter.Application
{
    public class SlayerGame
    {
        private readonly Random random = new Random();
        private readonly Dictionary<IPlayer, int> scores = new Dictionary<IPlayer, int>();
        private readonly ISpawnPointProvider spawnPointProvider;

        public SlayerGame(ISpawnPointProvider spawnPointProvider, IEnumerable<IPlayer> players)
        {
            this.spawnPointProvider = spawnPointProvider;

            foreach (var player in players)
            {
                player.Deaths.Subscribe(this.OnDeath);
            }
        }

        public void OnDeath(IKill kill)
        {
            this.scores[kill.Killer]++;

            var spawnPoint =
                this.spawnPointProvider.GetSpawnPoints()
                    .Where(x => x.SupportsPlayer(kill.Killed))
                    .OrderBy(x => this.random.NextDouble())
                    .FirstOrDefault();

            kill.Killed.Respawn(spawnPoint);
        }
    }
}