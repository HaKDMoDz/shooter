using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shooter.Core;
using Shooter.Core.Input;
using Shooter.Core.Input.Mouse;

namespace Shooter.Gameplay.Prefabs
{
    public class MouseDrivenThing : GameObject
    {
        private Body body;
        private FixedMouseJoint joint;

        public MouseDrivenThing(Engine engine)
            : base(engine)
        {
        }

        protected override void OnInitialize(ICollection<IDisposable> disposables)
        {
            this.body = BodyFactory.CreateCircle(this.Engine.World, 1f, 1f);
            this.body.BodyType = BodyType.Dynamic;
            this.body.IsBullet = true;
            this.joint = new FixedMouseJoint(this.body, Vector2.Zero);
            this.joint.MaxForce = 1000.0f * this.body.Mass;

            this.Engine.World.AddJoint(this.joint);

            disposables.Add(this.body);
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.Engine.Mouse.LeftButton.Where(x => x.State == ButtonState.Pressed).Subscribe(this.Press));
            attachments.Add(this.Engine.Mouse.Move.Subscribe(this.Move));
        }

        private void Press(MouseButtonAndState e)
        {

            if (e.Mouse.WorldPosition != null)
            {
                var pos = e.Mouse.WorldPosition.Value;
                this.Engine.Logger.Log(string.Format("Mouse Press: {0:000.000}x{1:000.000}", pos.X, pos.Y));
            }
        }

        private void Move(MouseMove e)
        {
            if (e.Mouse.WorldPosition != null)
            {
                var pos = e.Mouse.WorldPosition.Value;
                this.joint.WorldAnchorB = pos;
                this.Engine.Logger.Log(string.Format("Mouse Move: {0:000.000}x{1:000.000}", pos.X, pos.Y),
                                       TimeSpan.FromMilliseconds(100));
            }
        }
    }
}
