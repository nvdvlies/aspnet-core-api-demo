using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Invoice;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;
using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.Auditlogging
{
    internal class InvoiceAuditlogger : AuditlogBase<Invoice>, IAuditlog<Invoice>
    {
        public InvoiceAuditlogger(
            ICurrentUser currentUser, 
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUser, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(Invoice current, Invoice previous) =>
            new AuditlogBuilder<Invoice>()
                .WithProperty(c => c.InvoiceNumber)
                .WithProperty(c => c.CustomerId)
                .WithProperty(c => c.InvoiceDate, AuditlogType.DateOnly)
                .WithProperty(c => c.PaymentTerm)
                .WithProperty(c => c.OrderReference)
                .WithProperty(c => c.Status, customFormatter: InvoiceStatusEnumFormatter)
                .WithChildEntityCollection(c => c.InvoiceLines, new AuditlogBuilder<InvoiceLine>()
                    .WithProperty(c => c.Quantity)
                    .WithProperty(c => c.Description)
                    .WithProperty(c => c.SellingPrice, AuditlogType.Currency)
                )
                .Build(current, previous);

        private string InvoiceStatusEnumFormatter(Enum value)
        {
            var status = (InvoiceStatus)value;
            return status switch
            {
                InvoiceStatus.Draft => "Concept",
                InvoiceStatus.Sent => "Verzonden",
                InvoiceStatus.Paid => "Betaald",
                InvoiceStatus.Cancelled => "Geannuleerd",
                _ => value.ToString("G"),
            };
        }
    }
}
