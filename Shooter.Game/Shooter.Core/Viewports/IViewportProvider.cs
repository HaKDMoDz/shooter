using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Core
{
    public interface IViewportProvider
    {
        Viewport GetViewport(Rectangle rectangle);
    }
}