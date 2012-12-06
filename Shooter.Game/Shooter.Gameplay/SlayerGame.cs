using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Shooter.Core;
using Shooter.Gameplay.Logic;

namespace Shooter.Gameplay
{
    public class SlayerGame : GameObject
    {
        private readonly Dictionary<IPlayer, int> scores = new Dictionary<IPlayer, int>();
        private readonly ISpawnPointProvider spawnPointProvider;
        private readonly int scoreToWin;
        private readonly IObservable<IKill> deaths;
        private readonly List<IPlayer> players;

        public SlayerGame(Engine engine, ISpawnPointProvider spawnPointProvider, IEnumerable<IPlayer> players, int scoreToWin)
            : base(engine)
        {
            this.players = players.ToList();

            this.spawnPointProvider = spawnPointProvider;
            this.scoreToWin = scoreToWin;

            this.deaths = this.players.Select(x => x.Deaths).Merge().ObserveOn(this.Engine.PostPhysicsScheduler);

            this.GameOver = new Subject<Unit>();

            foreach (var player in this.players)
            {
                this.scores[player] = 0;
            }
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            foreach (var player in this.players)
            {
                player.Spawn(this.spawnPointProvider.GetSpawnPoint(player));
            }

            disposables.Add(this.deaths.Subscribe(this.OnDeath));
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            //attachments.Add(this.Engine.Draws
            //                    .ObserveOn(this.Engine.PostDrawScheduler)
            //                    .Subscribe(this.Draw));
        }

        public void OnDeath(IKill kill)
        {
            this.scores[kill.Killer]++;

            if (this.scores[kill.Killer] == this.scoreToWin)
            {
                this.GameOver.OnNext(Unit.Default);
            }

            kill.Killed.Spawn(this.spawnPointProvider.GetSpawnPoint(kill.Killed));
        }

        protected Subject<Unit> GameOver { get; private set; }
    }
}