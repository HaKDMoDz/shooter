namespace Shooter.Gameplay.Logic
{
    public class Kill: IKill
    {
        public Kill(IPlayer killed, IPlayer killer)
        {
            this.Killed = killed;
            this.Killer = killer;
        }

        public IPlayer Killed { get; set; }

        public IPlayer Killer { get; set; }
    }
}