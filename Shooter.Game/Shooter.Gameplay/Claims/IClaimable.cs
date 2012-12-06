using System;

namespace Shooter.Gameplay.Claims
{
    public interface IClaimable
    {
        IObserver<IClaimer> ClaimRequests { get; }
    }
}