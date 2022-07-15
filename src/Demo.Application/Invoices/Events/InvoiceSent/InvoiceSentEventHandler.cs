using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Invoices.Events.InvoiceSent
{
    public class InvoiceSentEventHandler : INotificationHandler<InvoiceSentEvent>
    {
        private readonly IEventHubContext _eventHubContext;
        private readonly ILogger<InvoiceSentEventHandler> _logger;

        public InvoiceSentEventHandler(
            ILogger<InvoiceSentEventHandler> logger,
            IEventHubContext eventHubContext)
        {
            _logger = logger;
            _eventHubContext = eventHubContext;
        }

        public Task Handle(InvoiceSentEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(InvoiceSentEvent)}");
            return _eventHubContext.All.InvoiceSent(@event.Data.Id);
        }
    }
}
