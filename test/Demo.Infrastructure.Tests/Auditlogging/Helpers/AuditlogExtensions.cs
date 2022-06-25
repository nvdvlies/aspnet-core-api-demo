using System.Linq;
using Demo.Domain.Auditlog;
using Xunit;

namespace Demo.Infrastructure.Tests.Auditlogging.Helpers
{
    public static class AssertExtensions
    {
        public static void AssertHasAuditlogItem(this Auditlog auditlog, string propertyName, string prevValue,
            string currentValue)
        {
            AuditlogItem auditlogItem = null;
            var propertyNameParts = propertyName.Split(".");
            foreach (var propertyNamePart in propertyNameParts)
            {
                auditlogItem = (auditlogItem != null ? auditlogItem.AuditlogItems : auditlog.AuditlogItems)?
                    .FirstOrDefault(x => x.PropertyName == propertyNamePart);
            }
            Assert.NotNull(auditlogItem);
            Assert.Equal(prevValue, auditlogItem.PreviousValueAsString);
            Assert.Equal(currentValue, auditlogItem.CurrentValueAsString);
        }
    }
}
