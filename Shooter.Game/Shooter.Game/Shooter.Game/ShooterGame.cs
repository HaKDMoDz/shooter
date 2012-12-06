using System;
using System.Collections;
using System.Reactive.Disposables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Gameplay;
using Shooter.Gameplay.Claims;
using Shooter.Gameplay.Levels;
using Shooter.Gameplay.Logic;
using Shooter.Gameplay.Menus.Models;
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
            this.Window.ClientSizeChanged +=
                (x, y) =>
                    {
                        this.graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
                        this.graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
                        this.graphics.ApplyChanges();
                    };

            GC.Collect();
        }

        protected override void Initialize()
        {
            this.engine = new Engine(this);

            var map = new OhSoSymmetrical(this.engine);

            var player1 = this.CreatePlayer("Player1", PlayerTeam.Red, PlayerIndex.One);
            var player2 = this.CreatePlayer("Player2", PlayerTeam.Blue, PlayerIndex.Two);
            var player3 = this.CreatePlayer("Player3", PlayerTeam.Green, PlayerIndex.Three);
            var player4 = this.CreatePlayer("Player4", PlayerTeam.Orange, PlayerIndex.Four);

            map.Initialize().Attach();

            new SlayerGame(this.engine, map, new[] {player1.Player, player2.Player, player3.Player, player4.Player}, 10)
                .Start();
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

        private PlayerStuffs CreatePlayer(string name, PlayerTeam team, PlayerIndex playerIndex)
        {
            var camera = new Camera(15.0f);
            var cameraController = new FollowingCameraController(this.engine, camera, null).Initialize().Attach();
            var player = new Player(name, team,
                                     (thePlayer, spawnPoint) =>
                                     {
                                         var playerRobot = new PlayerRobot(this.engine, thePlayer).Initialize().Attach();
                                         playerRobot.Position = spawnPoint.Position;
                                         cameraController.Target = playerRobot;

                                         // Default Weapon
                                         ((IClaimable) new Shotgun(this.engine).Initialize().Attach())
                                             .ClaimRequests.OnNext(playerRobot);

                                         return playerRobot;
                                     },
                                     playerRobot => new PlayerRobotController(this.engine, playerRobot, playerIndex).Initialize().Attach()
                );

            Perspective perspective;

            switch (playerIndex)
            {
                case PlayerIndex.One:
                    perspective = this.engine.PerspectiveManager
                        .CreatePerspective(
                            camera,
                            (bounds) =>
                            new Viewport(0, 0, bounds.Width / 2, bounds.Height / 2));
                    break;
                case PlayerIndex.Two:
                    perspective = this.engine.PerspectiveManager
                        .CreatePerspective(
                            camera,
                            (bounds) =>
                            new Viewport(bounds.Width / 2, 0, bounds.Width / 2, bounds.Height / 2));
                    break;
                case PlayerIndex.Three:
                    perspective = this.engine.PerspectiveManager
                        .CreatePerspective(
                            camera,
                            (bounds) =>
                                new Viewport(0, bounds.Height / 2, bounds.Width / 2, bounds.Height / 2));
                    break;
                default:
                    perspective = this.engine.PerspectiveManager
                        .CreatePerspective(
                            camera,
                            (bounds) =>
                            new Viewport(bounds.Width / 2, bounds.Height / 2, bounds.Width / 2, bounds.Height / 2));
                    break;
            }

            return new PlayerStuffs(player, camera, cameraController, perspective);
        }
    }

    public class PlayerStuffs : IDisposable
    {
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        public Player Player { get; private set; }
        public Camera Camera { get; private set; }
        public FollowingCameraController CameraController { get; private set; }
        public Perspective Perspective { get; private set; }

        public PlayerStuffs(Player player, Camera camera, FollowingCameraController cameraController, Perspective perspective)
        {
            this.Player = player;
            this.Camera = camera;
            this.CameraController = cameraController;
            this.Perspective = perspective;

            disposable.Add(player);
            disposable.Add(cameraController);
            disposable.Add(perspective);
        }

        public void Dispose()
        {
            this.disposable.Dispose();
        }
    }
}