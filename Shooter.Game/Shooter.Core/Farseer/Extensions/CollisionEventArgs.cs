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
}