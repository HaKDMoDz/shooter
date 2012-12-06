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
using Shooter.Gameplay.Menus.Models;
using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay.Logic
{
    public class PlayerRobot : GameObject, IClaimer, IContactDamagable, IPosition
    {
        private readonly IPlayer player;
        private readonly Subject<IKill> deaths = new Subject<IKill>();

        private Body body;
        private float turretRotation;
        private Vector2 movement;
        private IFireable weapon = Fireable.Empty;

        public PlayerRobot(Engine engine, IPlayer player)
            : base(engine)
        {
            this.player = player;
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.5f, 1);
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

            collisions.OfType<IContactDamager>()
                .Subscribe(this.ApplyDamage);

            this.claims.OfType<Weapon>()
                .Subscribe(this.ClaimWeapon);
        }

        public void ApplyDamage(IContactDamager damager)
        {
            if (health <= 0)
            {
                return;
            }

            this.health -= damager.GetContactDamage(this).Physical;

            if (health > 0)
            {
                return;
            }

            Console.WriteLine(this.health);
            this.Detach();
            this.deaths.OnNext(new Kill(this.player, damager.Player));
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

        public void Fire(float percent)
        {
            this.weapon.FireRequests.OnNext(new FireRequest(this.player, percent));
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
            set { this.body.Position = value; }
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