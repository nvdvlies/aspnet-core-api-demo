using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxMessage;

namespace Demo.Domain.OutboxMessage.Hooks
{
    internal class OutboxMessageCreatedHook : IAfterCreate<OutboxMessage>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IOutboxMessageCreatedEvents _outboxMessageCreatedEvents;

        public OutboxMessageCreatedHook(
            IOutboxMessageCreatedEvents outboxMessageCreatedEvents,
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _outboxMessageCreatedEvents = outboxMessageCreatedEvents;
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<OutboxMessage> context,
            CancellationToken cancellationToken = default)
        {
            _outboxMessageCreatedEvents.Add(OutboxMessageCreatedEvent.Create(_correlationIdProvider.Id,
                context.Entity.Id, _currentUserIdProvider.Id));
            return Task.CompletedTask;
        }
    }
}