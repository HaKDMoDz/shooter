using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Gameplay.Logic;

namespace Shooter.Application
{
    public class Player : GameObject, IPlayer
    {
        private readonly Subject<IKill> deaths = new Subject<IKill>();

        private float health = 100.0f;

        private Body body;

        private readonly BehaviorSubject<IPlayerFire> latestPlayerFire = new BehaviorSubject<IPlayerFire>(null);
        private float turretRotation;
        private Vector2 movement;

        public Player(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 1, 1);
            this.body.Enabled = false;
            this.body.BodyType = BodyType.Dynamic;
            disposables.Add(this.body);

            var collisions = this.body.OnCollisionAsObservable()
                .ObserveOn(this.Engine.PostPhysicsScheduler)
                .Select(x => x.FixtureB.Body.UserData);

            collisions
                .OfType<IPlayerFire>()
                .Subscribe(this.latestPlayerFire);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.Engine.Updates.Subscribe(this.Update));
        }

        public void Update(EngineTime engineTime)
        {
            this.body.LinearVelocity += this.movement * engineTime.Elapsed;
        }

        public IObservable<IKill> Deaths
        {
            get { return this.deaths; }
        }

        public void Respawn(ISpawnPoint spawnPoint)
        {
            this.Dispose();
            this.Initialize().Attach();
        }

        public void Fire(float percent)
        {

        }

        public void SetTurretRotation(float radians)
        {
            this.turretRotation = radians;
        }

        public void SetMovement(Vector2 movement)
        {
            this.movement = movement;
        }
    }
}