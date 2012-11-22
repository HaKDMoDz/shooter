using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Shooter.Core;
using Shooter.Core.Input;

namespace Shooter.UserInterface
{
    public class Class1
    {
    }

    public interface IControl
    {
        IControlList Parent { get; }

        IObservable<Unit> Move { get; }
        IObservable<Unit> MouseMove { get; }
        IObservable<Unit> MouseEnter { get; }
        IObservable<Unit> MouseExit { get; }
        IObservable<Unit> MouseDown { get; }
        IObservable<Unit> MouseUp { get; }
    }

    public interface IControlList : IList<IControl>
    {
    }

    public class Button : Control
    {
    }

    public class Control : IControl
    {
        private readonly Engine engine;
        internal IControlList Parent;

        private readonly Subject<MouseMove> move;
        private readonly Subject<Unit> mouseMove;
        private readonly IObservable<MouseMove> mouseEnter;
        private readonly IObservable<MouseMove> mouseExit;
        private readonly IObservable<MouseButtonAndState> mouseDown;
        private readonly Subject<Unit> mouseUp;
        private Rectangle Bounds;

        public Control(Engine engine)
        {
            this.engine = engine;
            this.Parent = null;
            this.move = this.engine.Mouse.Move;

            this.mouseMove = new Subject<Unit>();
            this.mouseDown = this.engine.Mouse.LeftButton.Where(x => this.Bounds.Contains(x.Mouse.ScreenPosition));
        }

        IControlList IControl.Parent
        {
            get { return this.Parent; }
        }

        IObservable<Unit> IControl.Move
        {
            get { return this.move; }
        }

        IObservable<Unit> IControl.MouseMove
        {
            get { return this.mouseMove; }
        }

        IObservable<Unit> IControl.MouseEnter
        {
            get { return this.mouseEnter; }
        }

        IObservable<Unit> IControl.MouseExit
        {
            get { return this.mouseExit; }
        }

        IObservable<Unit> IControl.MouseDown
        {
            get { return this.mouseDown; }
        }

        IObservable<Unit> IControl.MouseUp
        {
            get { return this.mouseUp; }
        }
    }
}
