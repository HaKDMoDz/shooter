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
    public class SampleLevel : GameObject
    {
        public SampleLevel(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            var v1 = new Vector2(-25, -25);
            var v2 = new Vector2(+25, -25);
            var v3 = new Vector2(+25, +25);
            var v4 = new Vector2(-50, +50);

            var body1 = this.MakeWall(v1, v2, 1f);
            var body2 = this.MakeWall(v2, v3, 1f);
            var body3 = this.MakeWall(v3, v4, 1f);
            var body4 = this.MakeWall(v4, v1, 1f);

            disposables.Add(body1);
            disposables.Add(body2);
            disposables.Add(body3);
            disposables.Add(body4);
        }

        private Body MakeWall(Vector2 a, Vector2 b, float border)
        {
            var diff = (a - b);
            var distance = diff.Length();
            var rotation = (float) Math.Atan2(diff.Y, diff.X);
            var width = distance + border * 2;
            var length = border * 2;

            var body = BodyFactory.CreateRectangle(this.Engine.World, width, length, 1f);

            body.Position = Vector2.Lerp(a, b, 0.5f);
            body.Rotation = rotation;

            return body;
        }
    }
}
