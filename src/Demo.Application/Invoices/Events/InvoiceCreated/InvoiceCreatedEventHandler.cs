using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Invoices.Events.InvoiceCreated
{
    public class InvoiceCreatedEventHandler : INotificationHandler<InvoiceCreatedEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<InvoiceCreatedEventHandler> _logger;

        public InvoiceCreatedEventHandler(
            ILogger<InvoiceCreatedEventHandler> logger,
            IEventHubContext eventHubContext)
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(InvoiceCreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(InvoiceCreatedEvent)}");
            await _eventHubContext.All.InvoiceCreated(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}