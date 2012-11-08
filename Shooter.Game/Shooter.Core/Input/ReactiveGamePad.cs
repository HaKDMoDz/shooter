using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shooter.Core.Input
{
    public class ReactiveGamePad
    {
        private readonly PlayerIndex playerIndex;

        public ReactiveGamePad(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public void Update()
        {
            var state = GamePad.GetState(this.playerIndex);
        }
    }
}
