using System;

namespace Shooter.Gameplay.Claims
{
    public interface IClaimable
    {
        bool IsClaimed { get; }
        IObserver<IClaimer> ClaimRequests { get; }
    }
}