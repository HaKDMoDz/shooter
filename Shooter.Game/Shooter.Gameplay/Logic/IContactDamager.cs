using FarseerPhysics.Dynamics;

namespace Shooter.Gameplay.Logic
{
    public interface IContactDamager
    {
        IPlayer Player { get; }
        IContactDamage GetContactDamage(IContactDamagable damagable);
    }
}