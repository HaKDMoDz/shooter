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

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    namespace Shooter.Core
    {
        public class RandomWeapon
        {
            Random RandomWeaponbers;
            double rocketgun = 0.05d; //1
            double flamegun = 0.20d;  //2
            double shotgun = 0.25d;   //3
            double machinegun = 0.5d; //4

            Engine engine;

            int cachedWeapon = 0;

            string gun = "My Gun";
            Weapon randomWeapon;


            public RandomWeapon(Engine engine)
            {
                this.engine = engine;

                for (int i = 0; i < 10; i++)
                {
                    this.RandomWeaponbers = new Random();
                }

            }

            public Weapon SelectGun()
            {
                double n = RandomWeaponbers.NextDouble();
                string gun;

                if (n <= rocketgun && this.cachedWeapon != 1)
                {

                    gun = "rocketgun";
                    randomWeapon = new Rocketlauncher(this.engine);
                    randomWeapon.Initialize().Attach();
                    ((Rocketlauncher)randomWeapon).Position = Vector2.One * 7;
                    this.cachedWeapon = 1;
                }
                else if (n <= (rocketgun + flamegun) && n > rocketgun && this.cachedWeapon != 2)
                {

                    gun = "flamegun";
                    randomWeapon = new Flamethrower(this.engine);
                    randomWeapon.Initialize().Attach();
                    ((Flamethrower)randomWeapon).Position = Vector2.One * 7;
                    this.cachedWeapon = 2;
                }
                else if (n <= (rocketgun + flamegun + shotgun) && n > (rocketgun + flamegun) && this.cachedWeapon != 3)
                {
                    gun = "shotgun";
                    randomWeapon = new Shotgun(this.engine);
                    randomWeapon.Initialize().Attach();
                    ((Shotgun)randomWeapon).Position = Vector2.One * 7;
                    this.cachedWeapon = 3;
                }
                else if (n <= (rocketgun + flamegun + shotgun + machinegun) && n > (rocketgun + flamegun + shotgun) && this.cachedWeapon != 4)
                {
                    gun = "machinegun";
                    randomWeapon = new Crossbow(this.engine);
                    randomWeapon.Initialize().Attach();
                    ((Crossbow)randomWeapon).Position = Vector2.One * 7;
                    this.cachedWeapon = 4;
                }
                else
                {
                    randomWeapon = SelectGun();
                }

                return randomWeapon;
            }


        }
    }

}
