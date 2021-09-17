using Xunit;

namespace Demo.WebApi.Tests.Helpers
{
    [CollectionDefinition(nameof(SharedFixture))]
    public class SharedFixtureCollection : ICollectionFixture<SharedFixture>
    { 
    }
}
