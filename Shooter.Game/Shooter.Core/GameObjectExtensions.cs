using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Core
{
    public static class GameObjectExtensions
    {
        public static T Initialize<T>(this T obj) where T : GameObject
        {
            obj.Initialize();
            return obj;
        }

        public static T Attach<T>(this T obj) where T : GameObject
        {
            obj.Attach();
            return obj;
        }
    }
}
