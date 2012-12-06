using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Shooter.Gameplay.Logic;
using Shooter.Gameplay.Menus.Models;

namespace Shooter.Gameplay
{
    public class Player : IPlayer, IDisposable
    {
        private readonly Func<IPlayer, ISpawnPoint, PlayerRobot> characterFactory;
        private readonly Func<PlayerRobot, PlayerRobotController> controllerFactory;
        private readonly CompositeDisposable characterDisposable = new CompositeDisposable();
        private readonly Subject<IKill> deaths = new Subject<IKill>();

        public string Name { get; private set; }
        public PlayerTeam Team { get; private set; }
        IObservable<IKill> IPlayer.Deaths { get { return this.deaths; } }

        public void Spawn(ISpawnPoint spawnPoint)
        {
            this.characterDisposable.Clear();

            var character = this.characterFactory(this, spawnPoint);
            var controller = this.controllerFactory(character);

            this.characterDisposable.Add(character.Deaths.Subscribe(this.deaths));
            this.characterDisposable.Add(character);
            this.characterDisposable.Add(controller);
        }

        public Player(string name, PlayerTeam team, Func<IPlayer, ISpawnPoint, PlayerRobot> characterFactory, Func<PlayerRobot, PlayerRobotController> controllerFactory)
        {
            this.Name = name;
            this.Team = team;
            this.characterFactory = characterFactory;
            this.controllerFactory = controllerFactory;
        }

        public void Dispose()
        {
            this.characterDisposable.Dispose();
        }
    }
}