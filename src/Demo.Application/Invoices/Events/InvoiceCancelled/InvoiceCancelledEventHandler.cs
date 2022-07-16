using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Invoices.Events.InvoiceCancelled;

public class InvoiceCancelledEventHandler : INotificationHandler<InvoiceCancelledEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<InvoiceCancelledEventHandler> _logger;

    public InvoiceCancelledEventHandler(
        ILogger<InvoiceCancelledEventHandler> logger,
        IEventHubContext eventHubContext
    )
    {
        _logger = logger;
        _eventHubContext = eventHubContext;
    }

    public Task Handle(InvoiceCancelledEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(InvoiceCancelledEvent)}");
        return _eventHubContext.All.InvoiceCancelled(@event.Data.Id);
    }
}
