using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Core.Input;
using Shooter.Core.Xna.Extensions;

namespace Shooter.Core
{
    public class Engine
    {
        private readonly DebugViewXNA worldView;
        private readonly Subject<EngineTime> updates = new Subject<EngineTime>();
        private readonly Subject<EngineTime> draws = new Subject<EngineTime>();
        private readonly Subject<EngineTime> perspectiveDraws = new Subject<EngineTime>();

        public Engine(Game game)
        {
            this.Game = game;
            this.World = new World(Vector2.Zero);
            this.PerspectiveManager = new PerspectiveManager();

            this.worldView = new DebugViewXNA(this.World);

            var now = DateTime.Now;

            this.InputScheduler = new HistoricalScheduler(now);
            this.UpdateScheduler = new HistoricalScheduler(now);
            this.PrePhysicsScheduler = new HistoricalScheduler(now);
            this.PostPhysicsScheduler = new HistoricalScheduler(now);
            this.PostDrawScheduler = new HistoricalScheduler(now);

            this.GamePad = new ReactiveGamePad(PlayerIndex.One);
            this.Keyboard = new ReactiveKeyboard(this.InputScheduler);
            this.Mouse = new ReactiveMouse(this);

            this.Logger = new DebugLogger(this);
            this.Logger.Initialize().Attach();

            this.worldView.LoadContent(this.Game.GraphicsDevice, this.Game.Content);

            this.SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this.UISpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        public SpriteBatch UISpriteBatch { get; set; }

        public SpriteBatch SpriteBatch { get; set; }


        public Game Game { get; private set; }
        public World World { get; private set; }
        public PerspectiveManager PerspectiveManager { get; set; }
        public ReactiveGamePad GamePad { get; set; }
        public ReactiveKeyboard Keyboard { get; private set; }
        public ReactiveMouse Mouse { get; private set; }
        public DebugLogger Logger { get; private set; }

        public IObservable<EngineTime> Updates
        {
            get { return this.updates; }
        }

        public IObservable<EngineTime> Draws
        {
            get { return this.draws; }
        }

        public IObservable<EngineTime> PerspectiveDraws
        {
            get { return this.perspectiveDraws; }
        } 

        public HistoricalScheduler InputScheduler { get; private set; }
        public HistoricalScheduler UpdateScheduler { get; private set; }
        public HistoricalScheduler PrePhysicsScheduler { get; private set; }
        public HistoricalScheduler PostPhysicsScheduler { get; private set; }

        public HistoricalScheduler PostDrawScheduler { get; private set; }

        public void Update(GameTime gt)
        {
            var now = DateTime.Now;
            var time = new EngineTime(gt, 1);

            this.updates.OnNext(time);

            this.GamePad.Update();
            this.Keyboard.Update();
            this.Mouse.Update();

            this.InputScheduler.AdvanceTo(now);

            this.UpdateScheduler.AdvanceTo(now);

            this.PrePhysicsScheduler.AdvanceTo(now);

            this.World.Step((float) gt.ElapsedGameTime.TotalSeconds);

            this.PostPhysicsScheduler.AdvanceTo(now);
        }

        public void Draw(GameTime gt)
        {
            var now = DateTime.Now;
            var time = new EngineTime(gt, 1);

            this.draws.OnNext(time);

            var oldViewport = this.Game.GraphicsDevice.Viewport;

            using (Disposable.Create(() => { this.Game.GraphicsDevice.Viewport = oldViewport; }))
            {
                foreach (var perspective in this.PerspectiveManager)
                {
                    this.Game.GraphicsDevice.Viewport = perspective.Viewport;
                    this.PerspectiveManager.CurrentPerspective = perspective;

                    var matrix = perspective.GetMatrix() * SpriteBatchExtensions.GetUndoMatrix(perspective.Viewport);

                    this.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, matrix);
                    this.UISpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    this.perspectiveDraws.OnNext(time);
                    this.SpriteBatch.End();
                    this.UISpriteBatch.End();

                    var physicsViewMatrix = perspective.GetMatrix();
                    this.worldView.RenderDebugData(ref physicsViewMatrix);
                }
            }

            this.Game.GraphicsDevice.Viewport = oldViewport;

            this.PostDrawScheduler.AdvanceTo(now);
        }
    }
}