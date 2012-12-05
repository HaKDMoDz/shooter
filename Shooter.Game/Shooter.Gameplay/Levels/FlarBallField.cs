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
            //Outer Borders
            var b1 = new Vector2(-60, 10);
            var b2 = new Vector2(60, 10);
            var b3 = new Vector2(60, -10);
            var b4 = new Vector2(-60, -10);
            //Previous Flag Barriers
            var f1_1 = new Vector2(-54, 9);
            var f1_2 = new Vector2(-54, 7);
            var f2_1 = new Vector2(-59, 4);
            var f2_2 = new Vector2(-57, 4);
            //Next Flag Barriers
            var f3_1 = new Vector2(59, -4);
            var f3_2 = new Vector2(57, -4);
            var f4_1 = new Vector2(54, -9);
            var f4_2 = new Vector2(54, -7);
            //Top Trapezoids
            var t1_1 = new Vector2(-4, 9);
            var t1_2 = new Vector2(-4, 6);
            var t1_3 = new Vector2(4, 6);
            var t1_4 = new Vector2(4.675f, 6.09f);
            var t1_5 = new Vector2(24, 10);
            //Bottom Trapezoid
            var t2_1 = new Vector2(4, -9);
            var t2_2 = new Vector2(4, -6);
            var t2_3 = new Vector2(-4, -6);
            var t2_4 = new Vector2(-4.675f, -6.09f);
            var t2_5 = new Vector2(-24, -10);
            //Previous Space Breakers
            var s1_1 = new Vector2(-54, -6);
            var s1_2 = new Vector2(-36, -6);
            var s1_3 = new Vector2(-36, 6);
            var s1_4 = new Vector2(-15, 6);
            //Next Space-Breakers
            var s2_1 = new Vector2(54, 6);
            var s2_2 = new Vector2(36, 6);
            var s2_3 = new Vector2(36, -6);
            var s2_4 = new Vector2(15, -6);

            float border = 0.5f;

            disposables.Add(this.MakeWall(b1, b2, border));
            disposables.Add(this.MakeWall(b2, b3, border));
            disposables.Add(this.MakeWall(b3, b4, border));
            disposables.Add(this.MakeWall(b4, b1, border));

            disposables.Add(this.MakeWall(f1_1, f1_2, border));
            disposables.Add(this.MakeWall(f2_1, f2_2, border));
            disposables.Add(this.MakeWall(f3_1, f3_2, border));
            disposables.Add(this.MakeWall(f4_1, f4_2, border));

            disposables.Add(this.MakeWall(t1_1, t1_2, border));
            disposables.Add(this.MakeWall(t1_2, t1_3, border));
            disposables.Add(this.MakeWall(t1_4, t1_5, border));

            disposables.Add(this.MakeWall(t2_1, t2_2, border));
            disposables.Add(this.MakeWall(t2_2, t2_3, border));
            disposables.Add(this.MakeWall(t2_4, t2_5, border));

            disposables.Add(this.MakeWall(s1_1, s1_2, border));
            disposables.Add(this.MakeWall(s1_2, s1_3, border));
            disposables.Add(this.MakeWall(s1_3, s1_4, border));

            disposables.Add(this.MakeWall(s2_1, s2_2, border));
            disposables.Add(this.MakeWall(s2_2, s2_3, border));
            disposables.Add(this.MakeWall(s2_3, s2_4, border));
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
