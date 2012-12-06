using Shooter.Gameplay.Logic;

namespace Shooter.Gameplay.Weapons
{
    public interface IFireRequest
    {
        IPlayer Player { get; }
        float Amount { get; }
    }
}