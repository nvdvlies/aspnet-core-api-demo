using Demo.Domain.Invoice.BusinessComponent.Events;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using Demo.Domain.Quotation.BusinessComponent.State;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Invoice.BusinessComponent.Hooks
{
    internal class SynchronizeInvoicePdfDomainEventHook : IAfterCreate<Invoice>, IAfterUpdate<Invoice>
    {
        private readonly IInvoiceToPdfModelMapper _invoiceToPdfModelMapper;

        public SynchronizeInvoicePdfDomainEventHook(IInvoiceToPdfModelMapper invoiceToPdfModelMapper)
        {
            _invoiceToPdfModelMapper = invoiceToPdfModelMapper;
        }

        public async Task ExecuteAsync(HookType type, IBusinessComponentContext<Invoice> context, CancellationToken cancellationToken)
        {
            if (type == HookType.AfterCreate)
            {
                // An invoice always has an accompanying PDF document. We can create this by requesting synchronization via a domain event.
                context.Entity.PdfIsSynced = false;
                context.PublishDomainEventAfterCommit(new SynchronizeInvoicePdfDomainEvent(context.Entity.Id));
            } 
            else if (type == HookType.AfterUpdate && context.Pristine.Status == InvoiceStatus.Draft)
            {
                var invoiceToPdfModel = await _invoiceToPdfModelMapper.MapAsync(context.Entity, cancellationToken);
                var hashcode = invoiceToPdfModel.GetHashCode();

                // Compare calculated hashcode against hashcode when PDF was last generated
                if (hashcode != context.Entity.PdfHashcode)
                {
                    context.State.TryGet(InvoiceStateKeys.ThrowIfPdfIsNotSynced, out bool throwIfPdfIsNotSynced);

                    if (throwIfPdfIsNotSynced)
                    {
                        // Prevent a possible infinite loop by not allowing synchronization when in the context of
                        // updating the invoice directly after PDF synchronization has occured. In this scenario the 
                        // hashcodes should have been equal.
                        throw new DomainException("Expected invoice PDF to have been synced");
                    }

                    // Changes made to entity DO affect PDF content. Requesting synchronization.
                    context.Entity.PdfIsSynced = false;
                    context.PublishDomainEventAfterCommit(new SynchronizeInvoicePdfDomainEvent(context.Entity.Id));
                } 
                else
                {
                    // Changes made to entity DIDNT affect PDF content. Not requesting synchronization.
                    context.Entity.PdfIsSynced = true;
                }
            }
        }
    }
}
