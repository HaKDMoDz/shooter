using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Shooter.Core;
using Shooter.Gameplay;

namespace Shooter.Application
{
    public class FollowingCameraController : GameObject
    {
        private readonly Engine engine;
        private readonly Camera camera;
        public IPosition Target { get; set; }

        public FollowingCameraController(Engine engine, Camera camera, IPosition target)
            : base(engine)
        {
            this.engine = engine;
            this.camera = camera;
            this.Target = target;
        }

        protected override void OnAttach(ICollection<IDisposable> attachments)
        {
            attachments.Add(this.engine.Updates
                                .ObserveOn(this.engine.PostPhysicsScheduler)
                                .Where(x => this.Target != null)
                                .Subscribe(this.Update));
        }

        private void Update(EngineTime engineTime)
        {
            this.camera.Position = this.Target.Position;
        }
    }
}