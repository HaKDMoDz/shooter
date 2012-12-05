using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Gameplay.Logic;

namespace Shooter.Gameplay.Levels
{
    public class OhSoSymmetrical : GameObject, ISpawnPointProvider
    {
        public OhSoSymmetrical(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            //Border Coordinates
            var b1 = new Vector2(-20, +20);
            var b2 = new Vector2(+20, +20);
            var b3 = new Vector2(+20, -20);
            var b4 = new Vector2(-20, -20);

            //Middle-Side Wall Coords
            var w1_1 = new Vector2(0, +20);
            var w1_2 = new Vector2(0, +15);
            var w2_1 = new Vector2(+15, 0);
            var w2_2 = new Vector2(+20, 0);
            var w3_1 = new Vector2(0, -15);
            var w3_2 = new Vector2(0, -20);
            var w4_1 = new Vector2(-20, 0);
            var w4_2 = new Vector2(-15, 0);

            //Home Base Wall Coords
            var h1_1 = new Vector2(-11, 16);
            var h1_2 = new Vector2(-11, 11);
            var h1_3 = new Vector2(-16, 11);
            var h1_4 = new Vector2(-11, 11);

            var h2_1 = new Vector2(11, -16);
            var h2_2 = new Vector2(11, -11);
            var h2_3 = new Vector2(11, -11);
            var h2_4 = new Vector2(16, -11);

            var h3_1 = new Vector2(11, 16);
            var h3_2 = new Vector2(11, 11);
            var h3_3 = new Vector2(16, 11);
            var h3_4 = new Vector2(11, 11);

            var h4_1 = new Vector2(-11, -16);
            var h4_2 = new Vector2(-11, -11);
            var h4_3 = new Vector2(-16, -11);
            var h4_4 = new Vector2(-11, -11);

            //Outer-Middle Walls
            var o1_1 = new Vector2(-3, 11);
            var o1_2 = new Vector2(3, 11);
            var o2_1 = new Vector2(-3, -11);
            var o2_2 = new Vector2(3, -11);
            var o3_1 = new Vector2(-11, -3);
            var o3_2 = new Vector2(-11, 3);
            var o4_1 = new Vector2(11, -3);
            var o4_2 = new Vector2(11, 3);

            //Center Walls
            var c1_1 = new Vector2(3.5f, 6.5f);
            var c1_2 = new Vector2(6.5f, 3.5f);
            var c2_1 = new Vector2(6.5f, -3.5f);
            var c2_2 = new Vector2(3.5f, -6.5f);
            var c3_1 = new Vector2(-3.5f, -6.5f);
            var c3_2 = new Vector2(-6.5f, -3.5f);
            var c4_1 = new Vector2(-6.5f, 3.5f);
            var c4_2 = new Vector2(-3.5f, 6.5f);

            float border = 0.75f;
            float cBorder = 0.40f;

            //Make Outer Borders
            disposables.Add(this.MakeWall(b1, b2, border));
            disposables.Add(this.MakeWall(b2, b3, border));
            disposables.Add(this.MakeWall(b3, b4, border));
            disposables.Add(this.MakeWall(b4, b1, border));

            //Make Middle-Side Walls Walls
            disposables.Add(this.MakeWall(w1_1, w1_2, border));
            disposables.Add(this.MakeWall(w2_1, w2_2, border));
            disposables.Add(this.MakeWall(w3_1, w3_2, border));
            disposables.Add(this.MakeWall(w4_1, w4_2, border));

            //Make Home Base Walls
            disposables.Add(this.MakeWall(h1_1, h1_2, border));
            disposables.Add(this.MakeWall(h1_3, h1_4, border));
            disposables.Add(this.MakeWall(h2_1, h2_2, border));
            disposables.Add(this.MakeWall(h2_3, h2_4, border));
            disposables.Add(this.MakeWall(h3_1, h3_2, border));
            disposables.Add(this.MakeWall(h3_3, h3_4, border));
            disposables.Add(this.MakeWall(h4_1, h4_2, border));
            disposables.Add(this.MakeWall(h4_3, h4_4, border));

            //Outer-Middle Walls
            disposables.Add(this.MakeWall(o1_1, o1_2, border));
            disposables.Add(this.MakeWall(o2_1, o2_2, border));
            disposables.Add(this.MakeWall(o3_1, o3_2, border));
            disposables.Add(this.MakeWall(o4_1, o4_2, border));

            //Center Walls
            disposables.Add(this.MakeWall(c1_1, c1_2, cBorder));
            disposables.Add(this.MakeWall(c2_1, c2_2, cBorder));
            disposables.Add(this.MakeWall(c3_1, c3_2, cBorder));
            disposables.Add(this.MakeWall(c4_1, c4_2, cBorder));

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


        public IEnumerable<ISpawnPoint> GetSpawnPoints()
        {
            yield break;
        }
    }
}