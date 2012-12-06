using Microsoft.Xna.Framework;
using Shooter.Gameplay.Logic;
using Shooter.Gameplay.Menus.Models;

namespace Shooter.Gameplay.Levels
{
    public class SpawnPoint : ISpawnPoint
    {
        public SpawnPoint(Vector2 position)
            : this(position, PlayerTeam.None)
        {
        }

        public SpawnPoint(Vector2 position, PlayerTeam team)
        {
            this.Position = position;
            this.Team = team;
        }

        public PlayerTeam Team { get; private set; }
        public Vector2 Position { get; private set; }

        public bool SupportsPlayer(IPlayer player)
        {
            return true;
        }
    }
}