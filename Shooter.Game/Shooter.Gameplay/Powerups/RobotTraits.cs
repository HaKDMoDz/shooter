namespace Shooter.Gameplay.Powerups
{
    public class RobotTraits
    {
        public RobotTraits()
        {
            this.AccelerationRate = 1f;
            this.FireRate = 1f;
            this.ReloadRate = 1f;
            this.RechargeRate = 1f;
        }

        public float RechargeRate { get; set; }
        public float ReloadRate { get; set; }
        public float FireRate { get; set; }
        public float AccelerationRate { get; set; }
    }
}