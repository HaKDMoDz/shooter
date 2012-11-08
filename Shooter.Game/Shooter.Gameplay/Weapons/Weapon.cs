using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shooter.Core;

namespace Shooter.Gameplay.Weapons
{
    public abstract class Weapon : GameObject
    {
        protected Weapon(Engine engine)
            : base(engine)
        {
        }
    }
}
