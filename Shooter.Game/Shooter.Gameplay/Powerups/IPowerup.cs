namespace Shooter.Gameplay.Powerups
{
    public interface IPowerup
    {
        void Process(RobotAttributes robot);

        bool ShouldRemove { get; }
    }
}