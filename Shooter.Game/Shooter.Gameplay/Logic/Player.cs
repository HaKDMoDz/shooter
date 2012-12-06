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
using Shooter.Gameplay.Claims;
using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay.Logic
{
    public class Player : GameObject, IPlayer, IClaimer, IContactDamagable
    {
        private readonly Subject<IKill> deaths = new Subject<IKill>();

        private Body body;

        private readonly BehaviorSubject<IPlayerFire> latestPlayerFire = new BehaviorSubject<IPlayerFire>(null);
        private float turretRotation;
        private Vector2 movement;
        private IFireable weapon = Fireable.Empty;

        public Player(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 1, 1);
            this.body.Enabled = false;
            this.body.BodyType = BodyType.Dynamic;
            this.body.LinearDamping = 10.0f;
            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.Engine.Updates.Subscribe(this.Update));

            var collisions = this.body.OnCollisionAsObservable()
                .ObserveOn(this.Engine.PostPhysicsScheduler)
                .Select(x => x.FixtureB.Body.UserData);

            collisions.OfType<Weapon>()
                .Subscribe(this.TryClaimWeapon);

            collisions.OfType<IPlayerFire>()
                .Subscribe(this.latestPlayerFire);

            collisions.OfType<IContactDamager>()
                .Select(x => x.GetContactDamage(this))
                .Subscribe(this.ApplyContactDamage);

            this.claims.OfType<Weapon>()
                .Subscribe(this.ClaimWeapon);
        }

        public void ApplyContactDamage(IContactDamage damage)
        {
            this.health -= damage.Physical;

            if (health <= 0)
            {
                this.Dispose();
            }
        }

        public void Update(EngineTime engineTime)
        {
            this.body.LinearVelocity += this.movement * 100 * engineTime.Elapsed;
            this.body.Rotation = this.turretRotation;
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
            this.weapon.FireRequests.OnNext(percent);
        }

        public void TryClaimWeapon(IClaimable claimable)
        {
            claimable.ClaimRequests.OnNext(this);
        }
        public void ClaimWeapon(IFireable weapon)
        {
            this.weapon = weapon;
        }

        public void SetTurretRotation(float radians)
        {
            this.turretRotation = radians;
        }

        public void SetMovement(Vector2 movement)
        {
            this.movement = movement;
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
        }

        public Vector2 LinearVelocity
        {
            get { return this.body.LinearVelocity; }
        }

        IObserver<IClaimable> IClaimer.Claims
        {
            get { return this.claims; }
        }

        private readonly Subject<IClaimable> claims = new Subject<IClaimable>();
        private float health = 100.0f;
    }
}