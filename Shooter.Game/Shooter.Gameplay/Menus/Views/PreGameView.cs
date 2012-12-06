using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Xna.Framework;
using Shooter.Core;

namespace Shooter.Gameplay.Menus.Views
{
    public class PreGameView : GameObject
    {

        public PreGameView(Engine engine)
            : base(engine)
        {
            this.BackRequests = Observable.Never<Unit>();
            this.GameStartRequests = Observable.Never<Unit>();
            this.TeamChangeRequests = Observable.Never<PlayerTeamChangeRequest>();
            this.PlayerActivationRequests = Observable.Never<PlayerActivationRequest>();
            this.MapChangeRequests = Observable.Never<MapChangeRequest>();
            this.GameTypeChangeRequests = Observable.Never<GameTypeChangeRequest>();

            foreach (var playerIndex in Enum.GetValues(typeof(PlayerIndex)).Cast<PlayerIndex>())
            {
                var gamepad = this.Engine.Input.GetGamePad(playerIndex);

                var index = playerIndex;
                
                // PlayerRobot Activation
                this.PlayerActivationRequests = this.PlayerActivationRequests.Merge(
                    gamepad.Buttons.A.Release.Select(x => new PlayerActivationRequest(index, true))
                    );

                this.PlayerActivationRequests = this.PlayerActivationRequests.Merge(
                    gamepad.Buttons.B.Release.Select(x => new PlayerActivationRequest(index, false))
                    );

                // Team Changes
                this.TeamChangeRequests = this.TeamChangeRequests.Merge(
                    gamepad.Buttons.LeftShoulder.Release
                        .Select(x => new PlayerTeamChangeRequest(index, Direction.Previous))
                    );

                this.TeamChangeRequests = this.TeamChangeRequests.Merge(
                    gamepad.Buttons.RightShoulder.Release
                        .Select(x => new PlayerTeamChangeRequest(index, Direction.Next))
                    );

                // Back & Start
                this.BackRequests = this.BackRequests.Merge(
                    gamepad.Buttons.Back.Select(x => Unit.Default)
                    );

                this.GameStartRequests = this.BackRequests.Merge(
                    gamepad.Buttons.Start.Select(x => Unit.Default)
                    );

                // Map Changes
                this.MapChangeRequests = this.MapChangeRequests.Merge(
                    gamepad.DPad.Up.Select(x => new MapChangeRequest(Direction.Previous))
                    );

                this.MapChangeRequests = this.MapChangeRequests.Merge(
                    gamepad.DPad.Down.Select(x => new MapChangeRequest(Direction.Next))
                    );

                // Game Type Changes
                this.GameTypeChangeRequests = this.GameTypeChangeRequests.Merge(
                    gamepad.DPad.Left.Select(x => new GameTypeChangeRequest(Direction.Previous))
                    );

                this.GameTypeChangeRequests = this.GameTypeChangeRequests.Merge(
                    gamepad.DPad.Right.Select(x => new GameTypeChangeRequest(Direction.Next))
                    );
            }
        }

        public IObservable<Unit> BackRequests { get; private set; }
        public IObservable<Unit> GameStartRequests { get; private set; }
        public IObservable<PlayerTeamChangeRequest> TeamChangeRequests { get; private set; }
        public IObservable<PlayerActivationRequest> PlayerActivationRequests { get; private set; }
        public IObservable<MapChangeRequest> MapChangeRequests { get; private set; }
        public IObservable<GameTypeChangeRequest> GameTypeChangeRequests { get; set; }

        public class PlayerTeamChangeRequest
        {
            public readonly PlayerIndex PlayerIndex;
            public readonly Direction Direction;

            public PlayerTeamChangeRequest(PlayerIndex playerIndex, Direction direction)
            {
                this.PlayerIndex = playerIndex;
                this.Direction = direction;
            }
        }
    }
}