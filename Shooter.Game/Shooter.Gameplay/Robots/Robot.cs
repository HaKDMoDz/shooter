using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using Shooter.Core.Xna.Extensions;
using Shooter.Gameplay.Claims;
using Shooter.Gameplay.Menus.Models;
using Shooter.Gameplay.Weapons;

namespace Shooter.Gameplay.Logic
{
    public class Robot : GameObject, IClaimer, IContactDamagable, IPosition
    {
        private readonly IPlayer player;
        private readonly IPerspective perspective;
        private readonly Subject<IKill> deaths = new Subject<IKill>();

        private Body body;
        private float turretRotation;
        private Vector2 movement;
        private Weapon weapon = null;

        public Robot(Engine engine, IPlayer player, IPerspective perspective)
            : base(engine)
        {
            this.player = player;
            this.perspective = perspective;
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 0.5f, 1);
            this.body.Enabled = false;
            this.body.BodyType = BodyType.Dynamic;
            this.body.LinearDamping = 10.0f;
            this.body.FixedRotation = true;
            disposables.Add(this.body);

            disposables.Add(Disposable.Create(() =>
                                                  {
                                                      if (weapon != null)
                                                      {
                                                          this.weapon.Dispose();
                                                      }
                                                  }));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.body.Enabled = true;
            attachments.Add(Disposable.Create(() => this.body.Enabled = false));

            attachments.Add(this.Engine.Updates.Subscribe(this.Update));

            var collisions = this.body.OnCollisionAsObservable()
                .ObserveOn(this.Engine.PostPhysicsScheduler)
                .Select(x => x.FixtureB.Body.UserData);


            attachments.Add(collisions.OfType<Weapon>()
                                .Subscribe(this.TryClaimWeapon));

            attachments.Add(collisions.OfType<IContactDamager>()
                                .Subscribe(this.ApplyDamage));

            attachments.Add(this.claims.OfType<Weapon>()
                                .Subscribe(this.ClaimWeapon));

            attachments.Add(this.Engine
                                .PerspectiveDraws
                                .Subscribe(this.Draw));


            attachments.Add(this.perspective.Draws.Subscribe(this.DrawHud));
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
            this.Dispose();
            this.deaths.OnNext(new Kill(this.player, damager.Player));
        }

        private void Update(EngineTime engineTime)
        {
            this.body.LinearVelocity += this.movement * 100 * engineTime.Elapsed;

            if(this.body.LinearVelocity.LengthSquared() > 0)
            {
                this.body.Rotation = this.body.LinearVelocity.ToAngle();
            }

            this.health = Math.Min(health + MaxHealth * engineTime.Elapsed / 60, MaxHealth);
        }

        private void Draw(EngineTime engineTime)
        {
            Texture2D texture;

            switch (this.player.Team)
            {
                case PlayerTeam.Red:
                    texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Robot-Red");
                    break;
                case PlayerTeam.Blue:
                    texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Robot-Blue");
                    break;
                case PlayerTeam.Green:
                    texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Robot-Green");
                    break;
                case PlayerTeam.Orange:
                    texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Robot-Orange");
                    break;
                default:
                    texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Robot");
                    break;
            }

            var size = texture.Size();
            var w = (int) size.X;
            var h = (int) size.Y;

            this.Engine.SpriteBatch.Draw(texture,
                                         this.body.Position,
                                         new Rectangle(w / 2, 0, w / 2, h / 2),
                                         Color.White,
                                         this.body.Rotation + MathHelper.PiOver2,
                                         size / 4,
                                         Vector2.One / size * 2,
                                         SpriteEffects.None,
                                         0f);

            this.Engine.SpriteBatch.Draw(texture,
                                         this.body.Position,
                                         new Rectangle(0, h / 2, w / 2, h / 2),
                                         Color.White,
                                         this.turretRotation + MathHelper.PiOver2,
                                         size / 4,
                                         Vector2.One / size * 2,
                                         SpriteEffects.None,
                                         0f);
        }

        private void DrawHud(EngineTime engineTime)
        {
            var texture = this.Engine.Game.Content.Load<Texture2D>("Textures/Health");

            var percent = this.health / MaxHealth;

            var color = Color.Lerp(Color.Red, Color.CornflowerBlue, percent);

            var viewport = this.perspective.GetViewport(this.Engine.PerspectiveManager.Bounds);

            this.Engine.UISpriteBatch.Draw(
                texture,
                new Vector2(viewport.Width/2.0f, viewport.Height/25.0f), 
                null,
                color,
                0f,
                texture.Size() / 2,
                Vector2.One / texture.Size() *
                new Vector2(percent * viewport.Width / 2.0f, viewport.Height/25.0f),
                SpriteEffects.None,
                0f);
        }

        public IObservable<IKill> Deaths
        {
            get { return this.deaths; }
        }

        public void Fire(float percent)
        {
            if (weapon != null)
            {
                this.weapon.FireRequests.OnNext(new FireRequest(this.player, percent, this.body));
            }
        }

        public void TryClaimWeapon(IClaimable claimable)
        {
            claimable.ClaimRequests.OnNext(this);
        }

        public void ClaimWeapon(Weapon weapon)
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

        float IClaimer.Rotation
        {
            get { return this.turretRotation; }
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
        private const float MaxHealth = 100.0f;
        private float health = MaxHealth;
    }
}