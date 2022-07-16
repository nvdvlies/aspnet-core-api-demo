using System.Linq;
using AutoFixture;

namespace Demo.WebApi.Tests.Helpers;

internal static class AutoFixtureFactory
{
    public static Fixture CreateAutofixtureWithDefaultConfiguration()
    {
        var autoFixture = new Fixture();
        autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => autoFixture.Behaviors.Remove(b));
        autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return autoFixture;
    }
}
