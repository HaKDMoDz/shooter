using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Shooter.Core.Farseer.Extensions;

namespace Shooter.Gameplay.Powerups
{
    public class SpeedBoost : GameObject, IPowerup
    {
        private Body body;
        private bool shouldRemove = false;

        public SpeedBoost(Engine engine)
            : base(engine)
        {
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateGear(this.Engine.World, 1f, 6, 50f, 0.5f, 1f);
            this.body.Enabled = false;
            this.body.UserData = this;
            this.body.IsSensor = true;
            this.body.AngularVelocity = MathHelper.Pi;

            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Select(x => x.FixtureB.Body.UserData)
                                .OfType<Robot>()
                                .Subscribe(this.Collision));
        }

        private void Collision(Robot robot)
        {
            Observable.Interval(TimeSpan.FromSeconds(15)).Take(1).Subscribe(x =>
                this.shouldRemove = true
                );

            this.Dispose();
        }

        public void Process(RobotTraits robot)
        {
            robot.AccelerationRate *= 2f;
        }

        public bool ShouldRemove
        {
            get { return this.shouldRemove; }
        }
    }
}
