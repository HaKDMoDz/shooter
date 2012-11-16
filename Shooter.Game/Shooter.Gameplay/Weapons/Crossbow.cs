using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
    public class Crossbow : Weapon
    {
        private Body body;
        private float projectileSpeed = 35f;
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
        }

        public void Fire(Unit unit)
        {
            var now = DateTime.UtcNow;

            if (now - lastFire < TimeSpan.FromMilliseconds(75))
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

            this.Fires.OnNext(Unit.Default);
            const float kickbackForce = -10;
            this.Kickbacks.OnNext(this.body.Rotation.RadiansToDirection() * kickbackForce);
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

            disposables.Add(this.FireRequests.Subscribe(this.Fire));
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
