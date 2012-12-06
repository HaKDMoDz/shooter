using Microsoft.Xna.Framework;

namespace Shooter.Core
{
    public interface IPerspective : IViewportProvider
    {
        Camera Camera { get; }
        Matrix GetMatrix(Rectangle rectangle);
        Rectangle2D GetBounds(Rectangle rectangle);
    }
}