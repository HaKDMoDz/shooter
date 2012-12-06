using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using FarseerPhysics.Dynamics;

namespace Shooter.Core.Farseer.Extensions
{
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
