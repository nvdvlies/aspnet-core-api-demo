using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Invoices.Events.InvoiceUpdated;

public class InvoiceUpdatedEventHandler : INotificationHandler<InvoiceUpdatedEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<InvoiceUpdatedEventHandler> _logger;

    public InvoiceUpdatedEventHandler(
        ILogger<InvoiceUpdatedEventHandler> logger,
        IEventHubContext eventHubContext)
    {
        _logger = logger;
        _eventHubContext = eventHubContext;
    }

    public Task Handle(InvoiceUpdatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(InvoiceUpdatedEvent)}");
        return _eventHubContext.All.InvoiceUpdated(@event.Data.Id, @event.Data.UpdatedBy);
    }
}
