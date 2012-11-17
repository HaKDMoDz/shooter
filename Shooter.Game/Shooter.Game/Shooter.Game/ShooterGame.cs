using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Input;
using Shooter.Gameplay;
using Shooter.Gameplay.Levels;
using Shooter.Gameplay.Menus;
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
            this.graphics.PreferredBackBufferWidth = this.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = this.GraphicsDevice.DisplayMode.Height;
            this.graphics.ToggleFullScreen();

            this.engine = new Engine(this);

            new SplashScreen(this.engine).Initialize().Attach();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            this.engine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0, 0);

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