using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay.Logic
{
    public struct FireRequest : IFireRequest
    {
        private readonly IPlayer player;
        private readonly float amount;

        public FireRequest(IPlayer player, float amount)
        {
            this.player = player;
            this.amount = amount;
        }

        public IPlayer Player
        {
            get { return this.player; }
        }

        public float Amount
        {
            get { return this.amount; }
        }
    }
}