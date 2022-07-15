using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Events.OutboxMessage;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.OutboxMessage.Events.OutboxMessageCreated
{
    public class OutboxMessageCreatedEventHandler : INotificationHandler<OutboxMessageCreatedEvent>
    {
        private readonly ILogger<OutboxMessageCreatedEventHandler> _logger;
        private readonly IOutboxMessageSender _outboxMessageSender;

        public OutboxMessageCreatedEventHandler(
            ILogger<OutboxMessageCreatedEventHandler> logger,
            IOutboxMessageSender outboxMessageSender
        )
        {
            _logger = logger;
            _outboxMessageSender = outboxMessageSender;
        }

        public Task Handle(OutboxMessageCreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {nameof(OutboxMessageCreatedEvent)}");
            return _outboxMessageSender.SendAsync(@event.Data.Id, cancellationToken);
        }
    }
}
