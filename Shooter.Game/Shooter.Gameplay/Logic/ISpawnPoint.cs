using Microsoft.Xna.Framework;

namespace Shooter.Gameplay.Logic
{
    public interface ISpawnPoint
    {
        Vector2 GetPosition();
        bool SupportsPlayer(IPlayer player);
    }
}