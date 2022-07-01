using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.OutboxEvent;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.OutboxEvents.Events.OutboxEventCreated
{
    public class OutboxEventCreatedEventHandler : INotificationHandler<OutboxEventCreatedEvent>
    {
        private readonly ILogger<OutboxEventCreatedEventHandler> _logger;
        private readonly IOutboxEventPublisher _outboxEventPublisher;

        public OutboxEventCreatedEventHandler(
            ILogger<OutboxEventCreatedEventHandler> logger,
            IOutboxEventPublisher outboxEventPublisher
        )
        {
            _logger = logger;
            _outboxEventPublisher = outboxEventPublisher;
        }

        public async Task Handle(OutboxEventCreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(OutboxEventCreatedEvent)}'");
            await _outboxEventPublisher.PublishAsync(@event.Data.Id, cancellationToken);
        }
    }
}