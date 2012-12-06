using FarseerPhysics.Dynamics;

namespace Shooter.Core.Farseer.Extensions
{
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
}