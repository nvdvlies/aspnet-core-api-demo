using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;

namespace Demo.Application.Invoices.Events.InvoiceDeleted
{
    public class InvoiceDeletedEventHandler : INotificationHandler<InvoiceDeletedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoiceDeletedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceDeletedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoiceDeleted(@event.Data.Id, @event.Data.DeletedBy);
        }
    }
}