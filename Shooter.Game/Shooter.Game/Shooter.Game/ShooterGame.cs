using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Gameplay;
using Shooter.Gameplay.Levels;
using Shooter.Gameplay.Logic;
using Shooter.Gameplay.Menus.Views;
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

            var player1 = new Player(this.engine).Initialize().Attach();
            var playerController1 = new PlayerController(this.engine, player1, PlayerIndex.One).Initialize().Attach();

            var player2 = new Player(this.engine).Initialize().Attach();
            var playerController2 = new PlayerController(this.engine, player2, PlayerIndex.Two).Initialize().Attach();

            var player3 = new Player(this.engine).Initialize().Attach();
            var playerController3 = new PlayerController(this.engine, player3, PlayerIndex.Three).Initialize().Attach();

            var player4 = new Player(this.engine).Initialize().Attach();
            var playerController4 = new PlayerController(this.engine, player4, PlayerIndex.Four).Initialize().Attach();

            var camera = new Camera();

            camera.VerticalUnits = 20f;

            var viewport = this.GraphicsDevice.Viewport;

            this.engine.PerspectiveManager.Perspectives.Add(new Perspective(camera, viewport));
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
            this.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            this.engine.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}