using System.Linq;
using Demo.Domain.Auditlog;
using Xunit;

namespace Demo.Infrastructure.Tests.Auditlogging.Helpers
{
    public static class AssertExtensions
    {
        public static void AssertHasAuditlogItem(this Auditlog auditlog, string propertyName, string prevValue, string currentValue)
        {
            var auditlogItem = auditlog.AuditlogItems.FirstOrDefault(x => x.PropertyName == propertyName);
            Assert.NotNull(auditlogItem);
            Assert.Equal(prevValue, auditlogItem.PreviousValueAsString);
            Assert.Equal(currentValue, auditlogItem.CurrentValueAsString);
        }
    }
}