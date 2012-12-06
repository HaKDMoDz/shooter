using System;
using System.Collections;
using System.Reactive.Linq;
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
            player1.Position = new Vector2(-15, 15);
            var playerController1 = new PlayerController(this.engine, player1, PlayerIndex.One).Initialize().Attach();

            var player2 = new Player(this.engine).Initialize().Attach();
            player2.Position = new Vector2(15, 15);
            var playerController2 = new PlayerController(this.engine, player2, PlayerIndex.Two).Initialize().Attach();

            var player3 = new Player(this.engine).Initialize().Attach();
            player3.Position = new Vector2(-15, -15);
            var playerController3 = new PlayerController(this.engine, player3, PlayerIndex.Three).Initialize().Attach();

            var player4 = new Player(this.engine).Initialize().Attach();
            player4.Position = new Vector2(15, -15);
            var playerController4 = new PlayerController(this.engine, player4, PlayerIndex.Four).Initialize().Attach();

            var deaths =
                Observable.Merge(
                    player1.Deaths,
                    player2.Deaths,
                    player3.Deaths,
                    player4.Deaths);

            deaths.Subscribe(x => Console.WriteLine("{0} destroyed {1}", x.Killer, x.Killed));

            new Shotgun(this.engine).Initialize().Attach().Position = new Vector2(-15, 15);
            new Shotgun(this.engine).Initialize().Attach().Position = new Vector2(15, 15);
            new Shotgun(this.engine).Initialize().Attach().Position = new Vector2(-15, -15);
            new Shotgun(this.engine).Initialize().Attach().Position = new Vector2(15, -15);

            new OhSoSymmetrical(this.engine).Initialize().Attach();

            var camera1 = new Camera();
            var camera2 = new Camera();
            var camera3 = new Camera();
            var camera4 = new Camera();

            camera1.VerticalUnits = 20f;
            camera2.VerticalUnits = 20f;
            camera3.VerticalUnits = 20f;
            camera4.VerticalUnits = 20f;

            var cameraController1 = new PlayerCameraController(this.engine, camera1, player1).Initialize().Attach();
            var cameraController2 = new PlayerCameraController(this.engine, camera2, player2).Initialize().Attach();
            var cameraController3 = new PlayerCameraController(this.engine, camera3, player3).Initialize().Attach();
            var cameraController4 = new PlayerCameraController(this.engine, camera4, player4).Initialize().Attach();

            var viewport = this.GraphicsDevice.Viewport;

            var w = viewport.Width / 2;
            var h = viewport.Height / 2;

            this.engine.PerspectiveManager.Perspectives.Add(new Perspective(camera1, new Viewport(0, 0, w, h)));
            this.engine.PerspectiveManager.Perspectives.Add(new Perspective(camera2, new Viewport(w, 0, w, h)));
            this.engine.PerspectiveManager.Perspectives.Add(new Perspective(camera3, new Viewport(0, h, w, h)));
            this.engine.PerspectiveManager.Perspectives.Add(new Perspective(camera4, new Viewport(w, h, w, h)));
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
            this.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            this.engine.Draw(gameTime);

            base.Draw(gameTime);
        }
    }

    public class PlayerCameraController : GameObject
    {
        private readonly Camera camera;
        private readonly Player player;

        public PlayerCameraController(Engine engine, Camera camera, Player player)
            : base(engine)
        {
            this.camera = camera;
            this.player = player;
        }

        protected override void OnAttach(System.Collections.Generic.ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Updates.Subscribe(x => this.camera.Position = this.player.Position));
        }
    }
}