using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Input.GamePad
{
    public class ReactiveGamePad
    {
        private readonly PlayerIndex playerIndex;

        public ReactiveGamePad(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;

            this.Buttons = new ReactiveGamePadButtons(this);
            this.DPad = new ReactiveGamePadDPad(this);
            this.ThumbSticks = new ReactiveGamePadThumbSticks(this);
            this.Triggers = new ReactiveGamePadTriggers(this);
        }

        public GamePadState State { get; private set; }
        public GamePadState OldState { get; private set; }

        public ReactiveGamePadButtons Buttons { get; private set; }
        public ReactiveGamePadDPad DPad { get; private set; }
        public ReactiveGamePadThumbSticks ThumbSticks { get; private set; }
        public ReactiveGamePadTriggers Triggers { get; private set; }

        public void Update()
        {
            this.OldState = this.State;
            this.State = Microsoft.Xna.Framework.Input.GamePad.GetState(this.playerIndex, GamePadDeadZone.Circular);

            this.Buttons.Update();
            this.DPad.Update();
            this.ThumbSticks.Update();
            this.Triggers.Update();
        }
    }
}
