using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Shooter.Core;

namespace Shooter.Gameplay.Weapons
{
    public class RandomWeaponSpawner : GameObject
    {
        private readonly List<Func<Weapon>> factories = new List<Func<Weapon>>();

        private static readonly Random Random = new Random();

        public RandomWeaponSpawner(Engine engine)
            : base(engine)
        {
            this.factories.Add(() =>
            {
                var weapon = new Shotgun(this.Engine).Initialize();
                weapon.Position = this.Position;
                return weapon;
            });

            this.factories.Add(() =>
            {
                var weapon = new Flamethrower(this.Engine).Initialize();
                weapon.Position = this.Position;
                return weapon;
            });
        }

        public Vector2 Position { get; set; }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(
                Observable.Interval(TimeSpan.FromSeconds(10))
                    .Select(x => Unit.Default)
                    .Subscribe(this.SpawnWeapon));
        }

        private void SpawnWeapon(Unit unit)
        {
            if (this.currentWeapon != null && !this.currentWeapon.IsClaimed)
            {
                // this.currentWeapon.Dispose();
            }

            this.currentWeapon = this.factories[Random.Next(0, this.factories.Count)]();

            currentWeapon.Attach();
        }

        protected Weapon currentWeapon { get; set; }
    }
}
