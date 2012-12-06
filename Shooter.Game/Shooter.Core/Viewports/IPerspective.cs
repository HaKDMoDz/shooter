using System;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;

namespace Shooter.Core
{
    public interface IPerspective : IViewportProvider
    {
        Camera Camera { get; }
        IObservable<EngineTime> Draws { get; }
        Matrix GetMatrix(Rectangle rectangle);
        Rectangle2D GetBounds(Rectangle rectangle);
    }
}