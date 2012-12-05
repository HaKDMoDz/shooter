using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Shooter.Core;
using Shooter.Gameplay.Menus.Views;

namespace Shooter.Gameplay.Menus.Models
{
    public class PreGameController : GameObject
    {
        private PreGameView view;

        private Dictionary<PlayerIndex, Player> players = new Dictionary<PlayerIndex, Player>(); 

        public PreGameController(Engine engine)
            : base(engine)
        {
            this.view = new PreGameView(engine);

            this.players.Add(PlayerIndex.One, new Player(PlayerIndex.One));
            this.players.Add(PlayerIndex.Two, new Player(PlayerIndex.Two));
            this.players.Add(PlayerIndex.Three, new Player(PlayerIndex.Three));
            this.players.Add(PlayerIndex.Four, new Player(PlayerIndex.Four));
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.view.Initialize();
            disposables.Add(Disposable.Create(() => this.view.Dispose()));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            this.view.Attach();

            attachments.Add(Disposable.Create(() => this.view.Detach()));

            attachments.Add(this.view.PlayerActivationRequests.Subscribe(this.PlayerActivation));
            attachments.Add(this.view.GameStartRequests
                                .Where(x => this.players.Count(player => player.Value.Active) >= 2)
                                .Subscribe(this.StartGame));
        }

        private void PlayerActivation(PlayerActivationRequest request)
        {
            players[request.PlayerIndex].Active = request.Active;
        }

        private void StartGame(Unit unit)
        {
            this.Dispose();
        }

        private class Player
        {
            public PlayerIndex PlayerIndex { get; private set; }
            public bool Active { get; set; }
            public PlayerTeam Team { get; set; }

            public Player(PlayerIndex playerIndex)
            {
                this.PlayerIndex = playerIndex;
            }
        }
    }
}
