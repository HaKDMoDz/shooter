using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Gameplay.Logic;
using Shooter.Gameplay.Menus.Models;
using Shooter.Gameplay.Prefabs;
using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay.Levels
{
    public class OhSoSymmetrical : GameObject, ISpawnPointProvider
    {
        private readonly List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        public OhSoSymmetrical(Engine engine)
            : base(engine)
        {
            this.spawnPoints.Add(new SpawnPoint(new Vector2(-15, 15), PlayerTeam.Red));
            this.spawnPoints.Add(new SpawnPoint(new Vector2(15, 15), PlayerTeam.Blue));
            this.spawnPoints.Add(new SpawnPoint(new Vector2(-15, -15), PlayerTeam.Green));
            this.spawnPoints.Add(new SpawnPoint(new Vector2(15, -15), PlayerTeam.Orange));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            // Border Coordinates
            var b1 = new Vector2(-20, +20);
            var b2 = new Vector2(+20, +20);
            var b3 = new Vector2(+20, -20);
            var b4 = new Vector2(-20, -20);

            // Middle-Side Wall Coords
            var w1_1 = new Vector2(0, +20);
            var w1_2 = new Vector2(0, +15);
            var w2_1 = new Vector2(+15, 0);
            var w2_2 = new Vector2(+20, 0);
            var w3_1 = new Vector2(0, -15);
            var w3_2 = new Vector2(0, -20);
            var w4_1 = new Vector2(-20, 0);
            var w4_2 = new Vector2(-15, 0);

            // Home Base Wall Coords
            var h1_1 = new Vector2(-11, 16);
            var h1_2 = new Vector2(-11, 11);
            var h1_3 = new Vector2(-16, 11);

            var h2_1 = new Vector2(11, -16);
            var h2_2 = new Vector2(11, -11);
            var h2_3 = new Vector2(16, -11);

            var h3_1 = new Vector2(11, 16);
            var h3_2 = new Vector2(11, 11);
            var h3_3 = new Vector2(16, 11);

            var h4_1 = new Vector2(-11, -16);
            var h4_2 = new Vector2(-11, -11);
            var h4_3 = new Vector2(-16, -11);

            // Outer-Middle Walls
            var o1_1 = new Vector2(-3, 11);
            var o1_2 = new Vector2(3, 11);
            var o2_1 = new Vector2(-3, -11);
            var o2_2 = new Vector2(3, -11);
            var o3_1 = new Vector2(-11, -3);
            var o3_2 = new Vector2(-11, 3);
            var o4_1 = new Vector2(11, -3);
            var o4_2 = new Vector2(11, 3);

            // Center Walls
            var c1_1 = new Vector2(3.5f, 6.5f);
            var c1_2 = new Vector2(6.5f, 3.5f);
            var c2_1 = new Vector2(6.5f, -3.5f);
            var c2_2 = new Vector2(3.5f, -6.5f);
            var c3_1 = new Vector2(-3.5f, -6.5f);
            var c3_2 = new Vector2(-6.5f, -3.5f);
            var c4_1 = new Vector2(-6.5f, 3.5f);
            var c4_2 = new Vector2(-3.5f, 6.5f);

            const float border = 0.75f;
            const float cBorder = 0.40f;

            // Make Outer Borders
            attachments.Add(this.MakeWallLoop(border, new[] {b1, b2, b3, b4}));

            // Make Middle-Side Walls Walls
            attachments.Add(this.MakeWall(w1_1, w1_2, border));
            attachments.Add(this.MakeWall(w2_1, w2_2, border));
            attachments.Add(this.MakeWall(w3_1, w3_2, border));
            attachments.Add(this.MakeWall(w4_1, w4_2, border));

            // Make Home Base Walls
            attachments.Add(this.MakeWallChain(border, new[] {h1_1, h1_2, h1_3}));
            attachments.Add(this.MakeWallChain(border, new[] {h2_1, h2_2, h2_3}));
            attachments.Add(this.MakeWallChain(border, new[] {h3_1, h3_2, h3_3}));
            attachments.Add(this.MakeWallChain(border, new[] {h4_1, h4_2, h4_3}));

            // Outer-Middle Walls
            attachments.Add(this.MakeWall(o1_1, o1_2, border));
            attachments.Add(this.MakeWall(o2_1, o2_2, border));
            attachments.Add(this.MakeWall(o3_1, o3_2, border));
            attachments.Add(this.MakeWall(o4_1, o4_2, border));

            // Center Walls
            attachments.Add(this.MakeWall(c1_1, c1_2, cBorder));
            attachments.Add(this.MakeWall(c2_1, c2_2, cBorder));
            attachments.Add(this.MakeWall(c3_1, c3_2, cBorder));
            attachments.Add(this.MakeWall(c4_1, c4_2, cBorder));

            attachments.Add(new RandomWeaponSpawner(this.Engine).Initialize().Attach());
        }

        private IDisposable MakeWall(Vector2 a, Vector2 b, float border)
        {
            var diff = (a - b);
            var distance = diff.Length();
            var rotation = (float)Math.Atan2(diff.Y, diff.X);
            var width = distance + border * 2;
            var length = border * 2;

            var body = BodyFactory.CreateRectangle(this.Engine.World, width, length, 1f);

            body.Position = Vector2.Lerp(a, b, 0.5f);
            body.Rotation = rotation;

            new Wall(this.Engine, a, b, border).Initialize().Attach();

            return body;
        }

        private IDisposable MakeWallJoint(Vector2 v, float border)
        {
            return Disposable.Empty;
        }

        private IDisposable MakeWallLoop(float border, IList<Vector2> positions)
        {
            var disposable = new CompositeDisposable();

            for (int i = 0; i < positions.Count; i++)
            {
                var a = positions[i];
                var b = positions[(i + 1) % positions.Count];

                disposable.Add(this.MakeWall(a, b, border));
            }

            return disposable;
        }

        private IDisposable MakeWallChain(float border, IList<Vector2> positions)
        {
            var disposable = new CompositeDisposable();

            for (int i = 0; i < positions.Count - 1; i++)
            {
                var a = positions[i];
                var b = positions[(i + 1) % positions.Count];

                disposable.Add(this.MakeWall(a, b, border));
            }

            return disposable;
        }



        public ISpawnPoint GetSpawnPoint(IPlayer player)
        {
            var spawnPoint = this.spawnPoints.FirstOrDefault(x => x.Team == player.Team);

            if (spawnPoint != null)
            {
                this.spawnPoints.Remove(spawnPoint);
                this.spawnPoints.Add(spawnPoint);
            }
            else
            {
                spawnPoint = new SpawnPoint(Vector2.Zero, player.Team);
            }

            return spawnPoint;
        }
    }
}