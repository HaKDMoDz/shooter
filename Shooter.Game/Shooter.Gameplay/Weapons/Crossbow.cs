using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Core.Xna.Extensions;
using Shooter.Gameplay.Weapons.Projectiles;

namespace Shooter.Gameplay.Weapons
{
    public class Crossbow : GameObject, IFireable
    {
        private Body body;
        private float projectileSpeed = 50f;
        private Robot owner;
        public Vector2 Position { get { return this.body.Position; } set { this.body.Position = value; } }

        public int magazineSize = 200;

        public int magazineCount = 200;
        private DateTime lastFire = DateTime.MinValue;

        public Crossbow(Engine engine)
            : base(engine)
        {
        }

        public void Reload()
        {
            if (this.magazineCount >= 0)
            {
                this.magazineCount = -1;
                Observable.Return(magazineSize)
                    .Delay(TimeSpan.FromSeconds(1))
                    .ObserveOn(this.Engine.UpdateScheduler)
                    .Subscribe(x => this.magazineCount = x);
            }
        }

        public void Fire()
        {
            if (magazineCount <= 0)
            {
                this.Reload();

                return;
            }

            var now = DateTime.UtcNow;

            if (now - lastFire < TimeSpan.FromMilliseconds(50))
            {
                return;
            }

            lastFire = now;

            magazineCount--;

            var newBolt = new Bolt(this.Engine);
            
            newBolt.Initialize().Attach();

            var direction = this.body.Rotation.RadiansToDirection();

            newBolt.Velocity = direction * this.projectileSpeed + this.owner.LinearVelocity;
            newBolt.Rotation = this.body.Rotation;
            newBolt.Position = this.body.Position + direction * 2f;
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.5f, 1f);
            this.body.IsSensor = true;
            this.body.UserData = this;

            disposables.Add(this.body);

            disposables.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.owner != null)
                                .Subscribe(this.LinkPhysics));

            disposables.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Where(x => this.owner != null)
                                .Subscribe(this.Update));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.owner = null;
            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.owner == null && x.FixtureB.Body.UserData is Robot)
                                .Select(x => (Robot) x.FixtureB.Body.UserData)
                                .Subscribe(this.SetOwner));
        }

        private void Update(EngineTime time)
        {
            var direction = this.Engine.Mouse.KnownWorldPosition - this.Position;
            this.body.Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        private void LinkPhysics(EngineTime time)
        {
            this.body.Position = this.owner.Position;
        }

        private void SetOwner(Robot robot)
        {
            this.Detach();
            this.owner = robot;
        }
    }
}
