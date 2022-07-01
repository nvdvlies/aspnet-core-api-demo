using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Invoices.Events.InvoicePaid
{
    public class InvoicePaidEventHandler : INotificationHandler<InvoicePaidEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<InvoicePaidEventHandler> _logger;

        public InvoicePaidEventHandler(
            ILogger<InvoicePaidEventHandler> logger,
            IEventHubContext eventHubContext)
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoicePaidEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(InvoicePaidEvent)}");
            await _eventHubContext.All.InvoicePaid(@event.Data.Id);
        }
    }
}