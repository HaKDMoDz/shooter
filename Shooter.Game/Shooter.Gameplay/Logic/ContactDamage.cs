namespace Shooter.Gameplay.Logic
{
    public struct ContactDamage : IContactDamage
    {
        private readonly float physical;

        public ContactDamage(float physical)
        {
            this.physical = physical;
        }

        public float Physical { get { return this.physical; } }
    }
}