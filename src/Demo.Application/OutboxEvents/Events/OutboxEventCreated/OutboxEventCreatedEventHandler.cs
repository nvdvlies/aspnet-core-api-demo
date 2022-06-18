using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.OutboxEvent;
using MediatR;

namespace Demo.Application.OutboxEvents.Events.OutboxEventCreated
{
    public class OutboxEventCreatedEventHandler : INotificationHandler<OutboxEventCreatedEvent>
    {
        private readonly IOutboxEventPublisher _outboxEventPublisher;

        public OutboxEventCreatedEventHandler(
            IOutboxEventPublisher outboxEventPublisher
        )
        {
            _outboxEventPublisher = outboxEventPublisher;
        }

        public async Task Handle(OutboxEventCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _outboxEventPublisher.PublishAsync(@event.Data.Id, cancellationToken);
        }
    }
}
