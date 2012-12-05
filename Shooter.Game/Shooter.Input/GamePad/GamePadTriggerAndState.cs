namespace Shooter.Input.GamePad
{
    public struct GamePadTriggerAndState
    {
        public readonly ReactiveGamePad GamePad;
        public readonly GamePadTrigger Trigger;
        public readonly float Amount;

        public GamePadTriggerAndState(ReactiveGamePad gamepad, GamePadTrigger trigger, float amount)
        {
            this.GamePad = gamepad;
            this.Amount = amount;
            this.Trigger = trigger;
        }
    }
}