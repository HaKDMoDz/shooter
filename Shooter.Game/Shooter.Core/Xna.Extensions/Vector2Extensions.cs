using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Shooter.Core.Xna.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 RadiansToDirection(this float radians)
        {
            return new Vector2((float) Math.Cos(radians), (float) Math.Sin(radians));
        }

        public static Vector2 DegreesToDirection(this float degrees)
        {
            return MathHelper.ToRadians(degrees).RadiansToDirection();
        }
    }
}
