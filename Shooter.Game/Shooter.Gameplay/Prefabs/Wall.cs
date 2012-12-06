using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core;
using Shooter.Core.Xna.Extensions;

namespace Shooter.Gameplay.Prefabs
{
    public class Wall : GameObject
    {
        private readonly Vector2 a;
        private readonly Vector2 b;
        private readonly float border;
        private Texture2D texture;

        public Wall(Engine engine, Vector2 a, Vector2 b, float border)
            : base(engine)
        {
            this.a = a;
            this.b = b;
            this.border = border;
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Wall");
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.PerspectiveDraws.Subscribe(this.Draw));
        }

        private void Draw(EngineTime time)
        {
            var position = Vector2.Lerp(a, b, 0.5f);
            var diff = (a - b);
            var length = diff.Length() + border * 2;
            var rotation = diff.ToAngle();

            this.Engine.SpriteBatch.Draw(this.texture, position, null, Color.White, rotation, this.texture.Size() / 2,
                                         Vector2.One / this.texture.Size() * new Vector2(length, this.border * 2), SpriteEffects.None, 0f);
        }
    }
}
