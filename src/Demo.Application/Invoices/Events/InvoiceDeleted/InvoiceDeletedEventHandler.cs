using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Invoices.Events.InvoiceDeleted;

public class InvoiceDeletedEventHandler : INotificationHandler<InvoiceDeletedEvent>
{
    private readonly IEventHubContext _eventHubContext;
    private readonly ILogger<InvoiceDeletedEventHandler> _logger;

    public InvoiceDeletedEventHandler(
        ILogger<InvoiceDeletedEventHandler> logger,
        IEventHubContext eventHubContext)
    {
        _logger = logger;
        _eventHubContext = eventHubContext;
    }

    public Task Handle(InvoiceDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {nameof(InvoiceDeletedEvent)}");
        return _eventHubContext.All.InvoiceDeleted(@event.Data.Id, @event.Data.DeletedBy);
    }
}
