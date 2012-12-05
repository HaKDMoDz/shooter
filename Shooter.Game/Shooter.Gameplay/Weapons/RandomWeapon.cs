using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Core.Farseer.Extensions;
using System.Reactive.Subjects;
using System.Reactive;

namespace Shooter.Gameplay.Weapons
{
    public class RandomWeapon
        {
        readonly Random _randomWeaponbers;
        private const double RocketLauncher = 0.1d; //1
        private const double Flamethrower = 0.15d; //2
        private const double Shotgun = 0.2d; //3
        private const double Crossbow = 0.25d; //4
        private const double Pistol = .30d; //5



        readonly Engine engine;
        int _cachedWeapon = 0;
        string _gun = "My Gun";
        Weapon _randomWeapon;


            public RandomWeapon(Engine engine)
            {
                this.engine = engine;

                for (int i = 0; i < 10; i++)
                {
                    this._randomWeaponbers = new Random();
                }

            }

            public Weapon SelectGun()
            {
                var VectorPostion = Vector2.UnitX * 10;
                double n = _randomWeaponbers.NextDouble();
                string gun;

                if (n <= RocketLauncher && this._cachedWeapon != 1)
                {

                    gun = "RocketLauncher";
                    _randomWeapon = new Rocketlauncher(this.engine);
                    _randomWeapon.Initialize().Attach();
                    ((Rocketlauncher)_randomWeapon).Position = VectorPostion;
                    this._cachedWeapon = 1;
                }
                else if (n <= (RocketLauncher + Flamethrower) && n > RocketLauncher && this._cachedWeapon != 2)
                {

                    gun = "Flamethrower";
                    _randomWeapon = new Flamethrower(this.engine);
                    _randomWeapon.Initialize().Attach();
                    ((Flamethrower)_randomWeapon).Position = VectorPostion;
                    this._cachedWeapon = 2;
                }
                else if (n <= (RocketLauncher + Flamethrower + Shotgun) && n > (RocketLauncher + Flamethrower) && this._cachedWeapon != 3)
                {
                    gun = "Shotgun";
                    _randomWeapon = new Shotgun(this.engine);
                    _randomWeapon.Initialize().Attach();
                    ((Shotgun)_randomWeapon).Position = VectorPostion;
                    this._cachedWeapon = 3;
                }
                else if (n <= (RocketLauncher + Flamethrower + Shotgun + Crossbow) && n > (RocketLauncher + Flamethrower + Shotgun) && this._cachedWeapon != 4)
                {
                    gun = "Crossbow";
                    _randomWeapon = new Crossbow(this.engine);
                    _randomWeapon.Initialize().Attach();
                    ((Crossbow)_randomWeapon).Position = VectorPostion;
                    this._cachedWeapon = 4;
                }

                else if (n <= (RocketLauncher + Flamethrower + Shotgun + Crossbow + Pistol) && n > (RocketLauncher + Flamethrower + Shotgun + Crossbow) && this._cachedWeapon != 5)
                {
                    gun = "Pistol";
                    _randomWeapon = new Pistol(this.engine);
                    _randomWeapon.Initialize().Attach();
                    ((Pistol)_randomWeapon).Position = VectorPostion;
                    this._cachedWeapon = 5;
                }
                else
                {
                    _randomWeapon = SelectGun();
                }

                return _randomWeapon;
            }


        }
    }

