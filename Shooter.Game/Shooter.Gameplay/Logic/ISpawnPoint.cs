using Microsoft.Xna.Framework;

namespace Shooter.Gameplay.Logic
{
    public interface ISpawnPoint
    {
        Vector2 Position { get; }
        bool SupportsPlayer(IPlayer player);
    }
}