using Demo.Common.Interfaces;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Domain.Invoice.BusinessComponent
{
    internal class InvoiceBusinessComponent : BusinessComponent<Invoice>, IInvoiceBusinessComponent
    {
        public InvoiceBusinessComponent(
            ILogger<InvoiceBusinessComponent> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<Invoice> dbCommand,
            IEnumerable<IDefaultValuesSetter<Invoice>> defaultValuesSetters,
            IEnumerable<IValidator<Invoice>> validators,
            IEnumerable<IBeforeCreate<Invoice>> beforeCreateHooks,
            IEnumerable<IAfterCreate<Invoice>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<Invoice>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<Invoice>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<Invoice>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<Invoice>> afterDeleteHooks,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            IJsonService<Invoice> jsonService,
            IAuditlog<Invoice> auditlog
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, publishDomainEventAfterCommitQueue, jsonService, auditlog)
        {
        }

        internal override Func<IQueryable<Invoice>, IIncludableQueryable<Invoice, object>> Includes => _ => _
            .Include(invoice => invoice.InvoiceLines);

        public void SetStatus(InvoiceStatus newStatus)
        {
            var currentStatus = Context.Entity.Status;

            if (newStatus == currentStatus)
            {
                return;
            }

            switch (newStatus)
            {
                case InvoiceStatus.Sent when currentStatus == InvoiceStatus.Draft:
                case InvoiceStatus.Paid when currentStatus == InvoiceStatus.Sent:
                case InvoiceStatus.Cancelled when currentStatus == InvoiceStatus.Draft || currentStatus == InvoiceStatus.Sent:
                case InvoiceStatus.Draft when currentStatus == InvoiceStatus.Cancelled:
                    Context.Entity.Status = newStatus;
                    break;
                default:
                    throw new DomainException($"Changing invoice status from '{currentStatus}' to '{newStatus}' is not allowed.");
            }
        }
    }
}
