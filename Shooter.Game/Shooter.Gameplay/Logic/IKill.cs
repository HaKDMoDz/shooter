namespace Shooter.Gameplay.Logic
{
    public interface IKill
    {
        IPlayer Killed { get; set; }
        IPlayer Killer { get; set; }
    }
}