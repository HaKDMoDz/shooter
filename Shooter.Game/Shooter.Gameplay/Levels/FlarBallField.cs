using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;

namespace Shooter.Gameplay.Levels
{
    public class FlarBallField : GameObject
    {
        public FlarBallField(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            var v1 = new Vector2(-60, 10);
            var v2 = new Vector2(60, 10);
            var v3 = new Vector2(60, -10);
            var v4 = new Vector2(-60, -10);

            float border = 0.5f;

            disposables.Add(this.MakeWall(v1, v2, border));
            disposables.Add(this.MakeWall(v2, v3, border));
            disposables.Add(this.MakeWall(v3, v4, border));
            disposables.Add(this.MakeWall(v4, v1, border));
        }

        private Body MakeWall(Vector2 a, Vector2 b, float border)
        {
            var diff = (a - b);
            var distance = diff.Length();
            var rotation = (float)Math.Atan2(diff.Y, diff.X);
            var width = distance + border * 2;
            var length = border * 2;

            var body = BodyFactory.CreateRectangle(this.Engine.World, width, length, 1f);

            body.Position = Vector2.Lerp(a, b, 0.5f);
            body.Rotation = rotation;

            return body;
        }
    }
}
