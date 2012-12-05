namespace Shooter.Gameplay.Menus.Views
{
    public class MapChangeRequest
    {
        public readonly Direction Direction;

        public MapChangeRequest(Direction direction)
        {
            this.Direction = direction;
        }
    }
}