using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Engine;
using Shooter.Engine.Core;
using Shooter.Engine.Scene;
using Shooter.Engine.Xna.Extensions;

namespace Shooter.Gameplay
{
    public class RobotView : IDrawableSceneObject
    {
        private readonly ShooterEngine engine;
        private readonly RobotModel robotModel;
        private Texture2D texture;

        public RobotView(ShooterEngine engine, RobotModel robotModel)
        {
            this.engine = engine;
            this.robotModel = robotModel;
            this.texture = engine.Game.Content.Load<Texture2D>("Textures/Player");
        }

        public bool Intersects(Rectangle2D bounds)
        {
            return true;
        }

        public void Draw(float dt)
        {
            engine.SpriteBatch.Draw(texture,
                       this.robotModel.Position,
                       new Rectangle(64, 0, 64, 64),
                       Color.White,
                       this.robotModel.LowerBodyRotation + MathHelper.ToRadians(90f),
                       texture.Size() / 4f,
                       Vector2.One / texture.Size() * 2,
                       SpriteEffects.None, 0f);

            engine.SpriteBatch.Draw(texture,
                       this.robotModel.Position,
                       new Rectangle(0, 64, 64, 64),
                       Color.White,
                       this.robotModel.TurretRotation + MathHelper.ToRadians(90f),
                       texture.Size() / 4f,
                       Vector2.One / texture.Size() * 2,
                       SpriteEffects.None, 0f);
        }
    }
}