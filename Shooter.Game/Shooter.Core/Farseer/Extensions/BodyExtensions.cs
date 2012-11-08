using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Shooter.Core.Farseer.Extensions
{
    public class CollisionEventArgs
    {
        public CollisionEventArgs(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            this.FixtureA = fixtureA;
            this.FixtureB = fixtureB;
            this.Contact = contact;
        }

        public Fixture FixtureA { get; private set; }
        public Fixture FixtureB { get; private set; }
        public Contact Contact { get; private set; }
    }

    public class SeperationEventArgs
    {
        public SeperationEventArgs(Fixture fixtureA, Fixture fixtureB)
        {
            this.FixtureA = fixtureA;
            this.FixtureB = fixtureB;
        }

        protected Fixture FixtureA { get; private set; }
        protected Fixture FixtureB { get; private set; }
    }

    public static class BodyExtensions
    {
        public static IObservable<CollisionEventArgs> OnCollisionAsObservable(this Body body)
        {
            return Observable.Create<CollisionEventArgs>(
                observer =>
                    {
                        OnCollisionEventHandler handler =
                            (a, b, contact) =>
                                {
                                    observer.OnNext(new CollisionEventArgs(a, b, contact));
                                    return true;
                                };

                        body.OnCollision += handler;

                        return Disposable.Create(() =>
                            body.OnCollision -= handler
                            );
                    });
        }

        public static IObservable<SeperationEventArgs> OnSeperationAsObservable(this Body body)
        {
            return Observable.Create<SeperationEventArgs>(
                observer =>
                    {
                        OnSeparationEventHandler handler =
                            (a, b) => observer.OnNext(new SeperationEventArgs(a, b));

                        body.OnSeparation += handler;

                        return Disposable.Create(() => body.OnSeparation -= handler);
                    });
        }
    }
}
