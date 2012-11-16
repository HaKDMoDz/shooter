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
    public class NewLevel : GameObject
    {
        public NewLevel(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            var v1 = new Vector2(-25, -25);
            var v2 = new Vector2(-50, -25);
            var v3 = new Vector2(-50, +25);
            var v4 = new Vector2(-25, +25);
            var v5 = new Vector2(-25, +50);
            var v6 = new Vector2(+25, +50);
            var v7 = new Vector2(+25, +25);
            var v8 = new Vector2(+50, +25);
            var v9 = new Vector2(+50, -25);
            var v10 = new Vector2(+25, -25);
            var v11 = new Vector2(+25, -50);
            var v12 = new Vector2(-25, -50);

            float border = 2.5f;

            disposables.Add(this.MakeWall(v1, v2, border));
            disposables.Add(this.MakeWall(v2, v3, border));
            disposables.Add(this.MakeWall(v3, v4, border));
            disposables.Add(this.MakeWall(v4, v5, border));
            disposables.Add(this.MakeWall(v5, v6, border));
            disposables.Add(this.MakeWall(v6, v7, border));
            disposables.Add(this.MakeWall(v7, v8, border));
            disposables.Add(this.MakeWall(v8, v9, border));
            disposables.Add(this.MakeWall(v9, v10, border));
            disposables.Add(this.MakeWall(v10, v11, border));
            disposables.Add(this.MakeWall(v11, v12, border));
            disposables.Add(this.MakeWall(v12, v1, border));
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
