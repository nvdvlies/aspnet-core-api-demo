using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;

namespace Demo.Application.Invoices.Events.InvoicePaid
{
    public class InvoicePaidEventHandler : INotificationHandler<InvoicePaidEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public InvoicePaidEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoicePaidEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.InvoicePaid(@event.Data.Id);
        }
    }
}
