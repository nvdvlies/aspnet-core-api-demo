using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Invoices.Events.InvoicePdfSynchronized
{
    public class InvoicePdfSynchronizedEventHandler : INotificationHandler<InvoicePdfSynchronizedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<InvoicePdfSynchronizedEventHandler> _logger;

        public InvoicePdfSynchronizedEventHandler(
            ILogger<InvoicePdfSynchronizedEventHandler> logger,
            IEventHubContext eventHubContext)
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(InvoicePdfSynchronizedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(InvoicePdfSynchronizedEvent)}");
            return _eventHubContext.All.InvoicePdfSynchronized(@event.Data.Id);
        }
    }
}
