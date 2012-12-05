using System;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Gameplay;
using Shooter.Gameplay.Levels;
using Shooter.Gameplay.Powerups;
using Shooter.Gameplay.Prefabs;
using Shooter.Gameplay.Weapons;

namespace Shooter.Application
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ShooterGame : Game
    {
        private Engine engine;
        private GraphicsDeviceManager graphics;
        private Robot robot;

        public ShooterGame()
        {
            this.Content.RootDirectory = "Content";

            this.graphics = new GraphicsDeviceManager(this);

            this.IsMouseVisible = true;

            this.Window.AllowUserResizing = true;

            GC.Collect();
        }

        protected override void Initialize()
        {
            this.engine = new Engine(this);

            var speedBoost = new SpeedBoost(engine);

            speedBoost.Initialize().Attach();

            speedBoost.Position = Vector2.One * -10;

            new MainMenu(this.engine).Initialize().Attach();
            new Tiler(this.engine).Initialize().Attach();

            new OhSoSymmetrical(this.engine).Initialize().Attach();
            
            //new MouseDrivenThing(this.engine).Initialize().Attach();
            robot = new Robot(this.engine);
            robot.Initialize().Attach();
            robot.Position = Vector2.One;
            var crossbow = new Crossbow(this.engine);
            var pistol = new Pistol(this.engine);
            var flamethrower = new Flamethrower(this.engine);

            //Crossbow Initialization
            crossbow.Initialize().Attach();

            crossbow.Position = Vector2.One*15;

            var shotgun = new Shotgun(this.engine);

            shotgun.Initialize().Attach();

            shotgun.Position = Vector2.Zero;

            //Pistol Intialization
            pistol.Initialize().Attach();

            pistol.Position = Vector2.One * 4;

            //Flamethrower Initialization
            flamethrower.Initialize().Attach();

            flamethrower.Position = Vector2.One * 13;

            this.engine.Logger.Log("Hello, World!");

            var random = new Random((int) (DateTime.UtcNow.Ticks % int.MaxValue));

            for (var i = 0; i < 100; i++)
            {
                var brick = (FlyingBrick) new FlyingBrick(this.engine).Initialize().Attach();

                float x = (float) random.NextDouble() * 6 - 3;
                float y = (float) random.NextDouble() * 6 - 3;

                brick.Position = new Vector2(x, y);
            }

            this.OnDeviceReset();
        }

        private void OnDeviceReset()
        {
            var camera1 = new Camera();
            var camera2 = new Camera();

            camera1.Zoom = 0.5f;
            camera2.Zoom = 0.5f;

            new KeyboardCameraController(this.engine, camera1).Initialize().Attach();
            new RobotCameraController(this.engine, this.robot, camera2).Initialize().Attach();

            var w = engine.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var h = engine.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            var hw = w;
            var hh = h;

            this.engine.PerspectiveManager.Perspectives.Add(
                new Perspective(
                    camera2,
                    new Viewport(0, 0, hw, hh)
                    )
                );
            //this.engine.PerspectiveManager.Perspectives.Add(
            //    new Perspective(
            //        camera2,
            //        new Viewport(hw, 0, hw, hh)
            //        )
            //    );
            //this.engine.PerspectiveManager.Perspectives.Add(
            //    new Perspective(
            //        camera1,
            //        new Viewport(0, hh, hw, hh)
            //        )
            //    );
            //this.engine.PerspectiveManager.Perspectives.Add(
            //    new Perspective(
            //        camera2,
            //        new Viewport(hw, hh, hw, hh)
            //        )
            //    );
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            this.engine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            this.engine.Draw(gameTime);

            base.Draw(gameTime);
        }
    }

    internal class RobotCameraController : GameObject
    {
        private readonly Camera camera;
        private readonly Robot robot;

        public RobotCameraController(Engine engine, Robot robot, Camera camera)
            : base(engine)
        {
            this.robot = robot;
            this.camera = camera;
        }

        protected override void OnAttach(System.Collections.Generic.ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Updates
                                .ObserveOn(this.Engine.PostPhysicsScheduler)
                                .Subscribe(this.Update));
        }

        private void Update(EngineTime time)
        {
            this.camera.Position = Vector2.Lerp(this.camera.Position,
                                                this.robot.Position + this.robot.LinearVelocity / 10, 0.5f);
        }
    }
}