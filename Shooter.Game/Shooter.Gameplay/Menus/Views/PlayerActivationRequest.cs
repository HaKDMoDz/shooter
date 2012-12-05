using Microsoft.Xna.Framework;

namespace Shooter.Gameplay.Menus.Views
{
    public class PlayerActivationRequest
    {
        public readonly PlayerIndex PlayerIndex;
        public readonly bool Active;

        public PlayerActivationRequest(PlayerIndex playerIndex, bool active)
        {
            this.PlayerIndex = playerIndex;
            this.Active = active;
        }
    }
}