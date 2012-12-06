using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics.Contacts;

namespace Shooter.Core
{
    public abstract class GameObject : IObservable<GameObject>, IDisposable
    {
        private readonly Subject<GameObject> subject = new Subject<GameObject>();
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly CompositeDisposable attachments = new CompositeDisposable();

        private readonly IDisposable disposable;

        protected Engine Engine { get; private set; }

        protected bool Attached { get; private set; }
        protected bool Initialized { get; private set; }

        public IObservable<Contact> Contact { get; set; }

        protected GameObject(Engine engine)
        {
            this.Engine = engine;

            // Ensures the engine never loses us, so long as we do not get disposed
            this.disposable = this.Engine.Updates.Subscribe();
        }

        internal GameObject Initialize()
        {
            this.OnInitialize(this.disposables);
            this.Initialized = true;
            this.subject.OnNext(this);

            return this;
        }

        internal GameObject Attach()
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException("Cannot Attach before Initializing Game Object");
            }

            if (this.Attached)
            {
                throw new InvalidOperationException("Game Object already Attached");
            }

            this.OnAttach(this.attachments);
            this.Attached = true;
            this.subject.OnNext(this);

            return this;
        }

        public GameObject Detach()
        {
            if (!this.Attached)
            {
                throw new InvalidOperationException("Game Object not Attached");
            }

            this.OnDetach();
            this.attachments.Clear();
            this.Attached = false;
            this.subject.OnNext(this);

            return this;
        }

        public void Dispose()
        {
            if (this.Attached)
            {
                this.Detach();
            }
            this.OnDispose();
            this.disposables.Clear();
            this.Initialized = false;
            this.subject.OnCompleted();
            this.disposable.Dispose();
        }

        protected virtual void OnInitialize(ICollection<IDisposable> disposables)
        {
        }

        protected virtual void OnAttach(ICollection<IDisposable> attachments)
        {
        }

        protected virtual void OnDetach()
        {
        }

        protected virtual void OnDispose()
        {
        }

        public IDisposable Subscribe(IObserver<GameObject> observer)
        {
            return this.subject.Subscribe(observer);
        }
    }
}