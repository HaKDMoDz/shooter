using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Gameplay.Logic;

namespace Shooter.Gameplay.Weapons.Projectiles
{
    public class Shot : GameObject, IContactDamager, IPlayerFire
    {
        private Body body;

        public Shot(Engine engine, IPlayer player)
            : base(engine)
        {
            this.Player = player;
        }

        public Vector2 Position
        {
            get { return this.body.Position; }
            set { this.body.Position = value; }
        }

        public Vector2 Velocity
        {
            get { return this.body.LinearVelocity; }
            set { this.body.LinearVelocity = value; }
        }

        public float Rotation
        {
            get { return this.body.Rotation; }
            set { this.body.Rotation = value; }
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateRectangle(this.Engine.World, 0.05f, 0.05f, 2f);

            this.body.BodyType = BodyType.Dynamic;
            this.body.IsBullet = true;
            this.body.UserData = this;
            this.body.Enabled = false;
            this.body.CollisionCategories = Category.Cat31;
            this.body.CollidesWith = Category.All ^ Category.Cat31;
            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            attachments.Add(Disposable.Create(() => this.body.Enabled = false));
            attachments.Add(this.body.OnCollisionAsObservable()
                                .Where(x => !x.FixtureB.IsSensor)
                                .ObserveOn(this.Engine.UpdateScheduler)
                                .Subscribe(this.OnCollision));
        }

        private void OnCollision(CollisionEventArgs args)
        {
            this.Dispose();
        }

        public IContactDamage GetContactDamage(IContactDamagable damagable)
        {
            return new ContactDamage(1.0f);
        }

        public IPlayer Player { get; private set; }
    }
}
