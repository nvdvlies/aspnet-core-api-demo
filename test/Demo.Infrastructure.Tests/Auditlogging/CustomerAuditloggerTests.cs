using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Customer;
using Demo.Infrastructure.Auditlogging;
using Demo.Infrastructure.Tests.Auditlogging.Helpers;
using Moq;
using Xunit;

namespace Demo.Infrastructure.Tests.Auditlogging
{
    public class CustomerAuditloggerTests : AuditloggerTestsBase
    {
        private readonly CustomerAuditlogger _subject;

        public CustomerAuditloggerTests()
        {
            _subject = new CustomerAuditlogger(CurrentUserMock.Object, DateTimeMock.Object, AuditlogDomainEntityMock.Object);
        }

        [Fact]
        public async Task CustomerAuditlogger_When_no_changes_are_made_It_should_not_create_auditlog_item()
        {
            // Arrange
            var previous = new Customer
            {
                Name = "Test1"
            };
            var current = new Customer
            {
                Name = "Test1"
            };

            // Act
            await _subject.CreateAuditLogAsync(current, previous, CancellationToken.None);

            // Assert
            VerifyCreateAsyncCall(Times.Never);
        }

        [Fact]
        public async Task CustomerAuditlogger_When_name_is_modified_It_should_create_auditlog_item()
        {
            // Arrange
            var previous = new Customer
            {
                Name = "Test1"
            };
            var current = new Customer
            {
                Name = "Test2"
            };

            // Act
            await _subject.CreateAuditLogAsync(current, previous, CancellationToken.None);

            // Assert
            Assert.Equal(nameof(Customer), CapturedAuditlog.EntityName);
            Assert.Single(CapturedAuditlog.AuditlogItems);
            CapturedAuditlog.AssertHasAuditlogItem(nameof(Customer.Name), previous.Name, current.Name);
            VerifyCreateAsyncCall(Times.Once);
        }
    }
}