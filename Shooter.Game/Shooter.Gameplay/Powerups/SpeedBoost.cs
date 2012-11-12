using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Xna.Framework;
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
            this.body = BodyFactory.CreateGear(this.Engine.World, 1f, 6, 0.15f, 0.2f, 1f);
            this.body.Enabled = false;
            this.body.UserData = this;

            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.body.OnCollisionAsObservable()
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => x.FixtureB.Body.UserData is Robot)
                                .Subscribe(this.Collision));

        }

        private void Collision(CollisionEventArgs args)
        {
            Observable.Interval(TimeSpan.FromSeconds(3)).Take(1).Subscribe(x =>
                this.shouldRemove = true
                );

            this.Dispose();
        }

        public void Process(RobotAttributes robot)
        {
            robot.AccelerationFactor *= 2f;
        }

        public bool ShouldRemove
        {
            get { return this.shouldRemove; }
        }
    }
}
