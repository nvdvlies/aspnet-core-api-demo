using Demo.Domain.Invoice.BusinessComponent.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Events.SynchronizeInvoicePdf
{
    public class SynchronizeInvoicePdfDomainEventHandler : INotificationHandler<SynchronizeInvoicePdfDomainEvent>
    {
        public SynchronizeInvoicePdfDomainEventHandler()
        {
        }

        public Task Handle(SynchronizeInvoicePdfDomainEvent @event, CancellationToken cancellationToken)
        {
            // TODO: Send to queue

            return Task.CompletedTask;
        }
    }
}
