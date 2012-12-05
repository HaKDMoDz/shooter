namespace Shooter.Gameplay.Menus.Views
{
    public class GameTypeChangeRequest
    {
        public readonly Direction Direction;

        public GameTypeChangeRequest(Direction direction)
        {
            this.Direction = direction;
        }
    }
}