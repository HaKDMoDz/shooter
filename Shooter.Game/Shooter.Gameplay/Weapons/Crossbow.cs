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
using System.Reactive.Subjects;
using System.Reactive;

namespace Shooter.Gameplay.Weapons
{
    public class Crossbow : Weapon
    {
        private Body body;
        private float projectileSpeed = 35f;
        private RobotOld owner;

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        private int magazineSize = 200;
        private int magazineCount = 200;

        public Crossbow(Engine engine)
            : base(engine)
        {
        }

        public void Reload(Unit unit)
        {
        }

        public void Fire(float unit)
        {
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

            disposables.Add(
                this.FireRequests.Take(1)
                    .Concat(
                        Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(1).Where(x => false).Select(x => 1.0f))
                    .Repeat()
                    .Subscribe(this.Fire));

            //disposables.Add(this.ReloadRequests.Subscribe(this.Reload));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.owner = null;
            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.owner == null && x.FixtureB.Body.UserData is RobotOld)
                                .Select(x => (RobotOld) x.FixtureB.Body.UserData)
                                .Subscribe(this.SetOwner));
        }

        private void Update(EngineTime time)
        {
            var direction = this.Engine.Input.GetMouse().KnownWorldPosition - this.Position;
            this.body.Rotation = (float)Math.Atan2(direction.Y, direction.X);

            //var state = this.Engine.GamePad.State.ThumbSticks.Next;

            //this.body.Rotation = (float)Math.Atan2(state.Y, state.X);
        }

        private void LinkPhysics(EngineTime time)
        {
            this.body.Position = this.owner.Position;
        }

        private void SetOwner(RobotOld robot)
        {
            this.Detach();
            this.owner = robot;
        }
    }
}
