using FarseerPhysics.Dynamics;
using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay.Logic
{
    public struct FireRequest : IFireRequest
    {
        private readonly IPlayer player;
        private readonly float amount;
        private readonly Body body;

        public FireRequest(IPlayer player, float amount, Body body)
        {
            this.player = player;
            this.amount = amount;
            this.body = body;
        }

        public IPlayer Player
        {
            get { return this.player; }
        }

        public float Amount
        {
            get { return this.amount; }
        }

        public Body Body
        {
            get { return this.body; }
        }
    }
}