using Demo.Common.Interfaces;
using Demo.Domain.Invoice.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice
{
    internal class InvoiceDomainEntity : DomainEntity<Invoice>, IInvoiceDomainEntity
    {
        public InvoiceDomainEntity(
            ILogger<InvoiceDomainEntity> logger,
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
            IEventOutboxProcessor eventOutboxProcessor,
            IMessageOutboxProcessor messageOutboxProcessor,
            IJsonService<Invoice> jsonService,
            IAuditlogger<Invoice> auditlogger
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, eventOutboxProcessor, messageOutboxProcessor, jsonService, auditlogger)
        {
        }

        internal override Func<IQueryable<Invoice>, IIncludableQueryable<Invoice, object>> Includes => _ => _
            .Include(invoice => invoice.InvoiceLines);

        public async Task GetAsNewCopyAsync(Guid id, CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(GetAsNewCopyAsync));
            var asNoTrackingSettingToRestore = Options.AsNoTracking;
            try
            {
                WithOptions(x => x.AsNoTracking = true);

                var entity = await DbCommand.GetAsync(id, Includes, cancellationToken);
                if (entity == null)
                {
                    throw new DomainEntityNotFoundException($"Entity with id '{id}' not found.");
                }

                entity.Id = default;
                entity.InvoiceNumber = default;
                entity.Status = InvoiceStatus.Draft;
                entity.InvoiceDate = DateTime.UtcNow.Date;
                entity.PdfIsSynced = false;
                entity.PdfChecksum = default;
                entity.InvoiceLines.ForEach(x => x.Id = default);
                (entity as IAuditableEntity).ClearCreatedAndLastModified();
                (entity as ISoftDeleteEntity).UndoMarkAsDeleted();

                Context.Entity = entity;
            }
            finally
            {
                WithOptions(x => x.AsNoTracking = asNoTrackingSettingToRestore);
                stopwatch.Stop();
            }
        }

        public async Task GetAsNewCreditAsync(Guid id, CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(GetAsNewCreditAsync));
            var asNoTrackingSettingToRestore = Options.AsNoTracking;
            try
            {
                WithOptions(x => x.AsNoTracking = true);

                var entity = await DbCommand.GetAsync(id, Includes, cancellationToken);
                if (entity == null)
                {
                    throw new DomainEntityNotFoundException($"Entity with id '{id}' not found.");
                }

                entity.Id = default;
                entity.InvoiceNumber = default;
                entity.Status = InvoiceStatus.Draft;
                entity.InvoiceDate = DateTime.UtcNow.Date;
                entity.PdfIsSynced = false;
                entity.PdfChecksum = default;
                entity.InvoiceLines.ForEach(x =>
                {
                    x.Id = default;
                    x.Quantity *= -1;
                });
                (entity as IAuditableEntity).ClearCreatedAndLastModified();
                (entity as ISoftDeleteEntity).UndoMarkAsDeleted();

                Context.Entity = entity;
            }
            finally
            {
                WithOptions(x => x.AsNoTracking = asNoTrackingSettingToRestore);
                stopwatch.Stop();
            }
        }

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
                case InvoiceStatus.Cancelled when currentStatus == InvoiceStatus.Draft
                                               || currentStatus == InvoiceStatus.Sent:
                case InvoiceStatus.Draft when currentStatus == InvoiceStatus.Cancelled:
                    Context.Entity.Status = newStatus;
                    break;
                default:
                    throw new DomainException($"Changing invoice status from '{currentStatus}' to '{newStatus}' is not allowed.");
            }
        }
    }
}
