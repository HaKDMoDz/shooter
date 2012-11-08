using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Shooter.Core
{
    public class EngineTime
    {
        public GameTime GameTime { get; private set; }

        public float Factor { get; private set; }
        public float Total { get; private set; }
        public float Elapsed { get; private set; }
        public float ElapsedFactor { get; private set; }

        public EngineTime(GameTime gameTime, float factor)
        {
            this.GameTime = gameTime;
            this.Factor = factor;
            this.Total = (float) this.GameTime.TotalGameTime.TotalSeconds;
            this.Elapsed = (float) this.GameTime.ElapsedGameTime.TotalSeconds;
            this.ElapsedFactor = this.Elapsed*this.Factor;
        }
    }
}
