using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using Microsoft.Xna.Framework;
using Shooter.Input.GamePad;
using Shooter.Input.Keyboard;
using Shooter.Input.Mouse;

namespace Shooter.Input
{
    public class ReactiveInputManager
    {
        private ReactiveMouse mouse;

        private readonly Dictionary<PlayerIndex, ReactiveKeyboard> keyboards =
            new Dictionary<PlayerIndex, ReactiveKeyboard>();

        private readonly Dictionary<PlayerIndex, ReactiveGamePad> gamepads =
            new Dictionary<PlayerIndex, ReactiveGamePad>();

        public IScheduler Scheduler { get; set; }

        public ReactiveInputManager()
            : this(ReactiveInputManager.DefaultScheduler)
        {
        }

        public ReactiveInputManager(IScheduler scheduler)
        {
            this.Scheduler = scheduler;
        }

        public ReactiveKeyboard GetKeyboard(PlayerIndex playerIndex)
        {
            if (!keyboards.ContainsKey(playerIndex))
            {
                keyboards[playerIndex] = new ReactiveKeyboard(playerIndex, this.Scheduler);
            }

            return keyboards[playerIndex];
        }

        public ReactiveGamePad GetGamePad(PlayerIndex playerIndex)
        {
            if (!gamepads.ContainsKey(playerIndex))
            {
                gamepads[playerIndex] = new ReactiveGamePad(playerIndex, this.Scheduler);
            }

            return gamepads[playerIndex];
        }

        public ReactiveMouse GetMouse()
        {
            if (mouse == null)
            {
                mouse = new ReactiveMouse(this.Scheduler);
            }

            return mouse;
        }

        public static IScheduler DefaultScheduler
        {
            get { return System.Reactive.Concurrency.Scheduler.Default; }
        }

        public void Update()
        {
            foreach (var keyboard in keyboards.Values.ToList())
            {
                keyboard.Update();
            }

            if (mouse != null)
            {
                mouse.Update();
            }

            foreach (var gamepad in gamepads.Values.ToList())
            {
                gamepad.Update();
            }
        }
    }
}