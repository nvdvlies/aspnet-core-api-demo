using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Customer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Demo.Domain.Tests.Customer
{
    public class CustomerDomainEntityTests : DomainEntityTestsBase<Domain.Customer.Customer>
    {
        private readonly ICustomerDomainEntity _subject;

        public CustomerDomainEntityTests()
        {
            _subject = ServiceProvider.GetService<ICustomerDomainEntity>();
        }

        [Fact]
        public async Task CustomerDomainEntity_Create()
        {
            // Arrange

            // Act
            await _subject.NewAsync(CancellationToken.None);
            await _subject.CreateAsync(CancellationToken.None);

            // Assert
        }
    }
}