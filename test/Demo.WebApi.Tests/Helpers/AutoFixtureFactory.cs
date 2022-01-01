using AutoFixture;
using System.Linq;

namespace Demo.WebApi.Tests.Helpers
{
    internal class AutoFixtureFactory
    {
        public static Fixture CreateAutofixtureWithDefaultConfiguration()
        {
            var autoFixture = new Fixture();
            autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => autoFixture.Behaviors.Remove(b));
            autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return autoFixture;
        }
    }
}
