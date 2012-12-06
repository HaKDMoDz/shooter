using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Shooter.Core;
using Shooter.Gameplay.Logic;
using Shooter.Gameplay.Menus.Models;

namespace Shooter.Gameplay
{
    public class Player : IPlayer, IDisposable
    {
        private readonly IPerspective perspective;
        private readonly Func<IPlayer, ISpawnPoint, Robot> characterFactory;
        private readonly Func<Robot, RobotController> controllerFactory;
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

        public Player(string name, PlayerTeam team, IPerspective perspective, Func<IPlayer, ISpawnPoint, Robot> characterFactory, Func<Robot, RobotController> controllerFactory)
        {
            this.Name = name;
            this.Team = team;
            this.perspective = perspective;
            this.characterFactory = characterFactory;
            this.controllerFactory = controllerFactory;
        }

        public void Dispose()
        {
            this.characterDisposable.Dispose();
        }
    }
}