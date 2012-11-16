namespace Shooter.Gameplay.Powerups
{
    public interface IPowerup
    {
        void Process(RobotTraits robot);

        bool ShouldRemove { get; }
    }
}