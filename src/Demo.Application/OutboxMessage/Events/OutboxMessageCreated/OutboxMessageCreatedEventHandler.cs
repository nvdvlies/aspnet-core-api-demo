using Demo.Application.Shared.Interfaces;
using Demo.Events.OutboxMessage;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.OutboxMessage.Events.OutboxMessageCreated
{
    public class OutboxMessageCreatedEventHandler : INotificationHandler<OutboxMessageCreatedEvent>
    {
        private readonly IOutboxMessageSender _outboxMessageSender;

        public OutboxMessageCreatedEventHandler(
            IOutboxMessageSender outboxMessageSender
        )
        {
            _outboxMessageSender = outboxMessageSender;
        }

        public async Task Handle(OutboxMessageCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _outboxMessageSender.SendAsync(@event.Data.Id, cancellationToken);
        }
    }
}
