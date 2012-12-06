using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using System.Reactive.Subjects;
using System.Reactive;
using Shooter.Core.Xna.Extensions;
using Shooter.Gameplay.Claims;
using Shooter.Gameplay.Weapons.Projectiles;

namespace Shooter.Gameplay.Weapons
{
    public class Pistol : Weapon
    {
        private Body body;
        private IClaimer claimer;
        private const float BulletSpeed = 15;

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        public Pistol(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.5f, 1f);
            this.body.Enabled = false;
            this.body.IsSensor = true;
            this.body.UserData = this;

            disposables.Add(this.body);

            disposables.Add(
                this.FireRequests.Take(1)
                    .Concat(
                        Observable.Interval(TimeSpan.FromMilliseconds(100)).Take(1)
                            .Where(x => false)
                            .Select<long, IFireRequest>(x => null))
                    .Repeat()
                    .ObserveOn(this.Engine.PostPhysicsScheduler)
                    .Subscribe(this.Fire));

            disposables.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Where(x => this.claimer != null)
                                .Subscribe(this.LinkPhysics));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.claimer = null;
            this.body.Enabled = true;

            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.ClaimRequests
                                .Where(x => this.claimer == null)
                                .Subscribe(this.Claim));
        }

        public void Fire(IFireRequest request)
        {
            var shot = new Shot(this.Engine, request).Initialize().Attach();

            shot.Position = this.body.Position;
            shot.Velocity = this.claimer.LinearVelocity + this.body.Rotation.RadiansToDirection() * BulletSpeed;
        }

        private void LinkPhysics(EngineTime time)
        {
            this.body.Position = this.claimer.Position;
            this.body.Rotation = this.claimer.Rotation;
        }

        private void Claim(IClaimer claimer)
        {
            this.Detach();
            this.claimer = claimer;
            this.claimer.Claims.OnNext(this);
        }
    }
}
