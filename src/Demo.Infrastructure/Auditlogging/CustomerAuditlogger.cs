using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Customer;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;
using System.Collections.Generic;

namespace Demo.Infrastructure.Auditlogging
{
    internal class CustomerAuditlogger : AuditloggerBase<Customer>, IAuditlogger<Customer>
    {
        public CustomerAuditlogger(
            ICurrentUser currentUser,
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUser, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(Customer current, Customer previous) =>
            new AuditlogBuilder<Customer>()
                .WithProperty(c => c.Name)
                .WithProperty(c => c.InvoiceEmailAddress)
                .Build(current, previous);
    }
}
