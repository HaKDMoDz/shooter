namespace Shooter.Gameplay.Weapons
{
    public class Fireable
    {
        public static readonly IFireable Empty = new EmptyFireable();

        private class EmptyFireable : IFireable
        {
            public void Fire()
            {
            }

            public void Reload()
            {
            }
        }
    }
}