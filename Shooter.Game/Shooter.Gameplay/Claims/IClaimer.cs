using System;
using Microsoft.Xna.Framework;

namespace Shooter.Gameplay.Claims
{
    public interface IClaimer
    {
        Vector2 Position { get; }
        float Rotation { get; }
        Vector2 LinearVelocity { get; }

        IObserver<IClaimable> Claims { get; }
    }
}