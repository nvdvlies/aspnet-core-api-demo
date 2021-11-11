using AutoFixture;
using Demo.Domain.Customer;

namespace Demo.WebApi.Tests.Helpers
{
    internal class AutoFixtureFactory
    {
        public static Fixture CreateAutofixtureWithDefaultConfiguration()
        {
            var autoFixture = new Fixture();
            autoFixture.Customize<Customer>(composer =>
            {
                composer.Without(p => p.Invoices);
                return composer;
            });
            return autoFixture;
        }
    }
}
