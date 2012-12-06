using FarseerPhysics.Dynamics;

namespace Shooter.Gameplay.Logic
{
    public interface IContactDamager
    {
        IContactDamage GetContactDamage(IContactDamagable damagable);
    }

    public struct ContactDamage : IContactDamage
    {
        private readonly float physical;

        public ContactDamage(float physical)
        {
            this.physical = physical;
        }

        public float Physical { get { return this.physical; } }
    }

    public interface IContactDamage
    {
        float Physical { get; }
    }

    public interface IContactDamagable
    {
        
    }
}