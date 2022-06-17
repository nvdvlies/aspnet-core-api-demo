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
        private readonly ICurrentUser _currentUser;
        private readonly IOutboxMessageCreatedEvents _outboxMessageCreatedEvents;

        public OutboxMessageCreatedHook(
            IOutboxMessageCreatedEvents outboxMessageCreatedEvents,
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _outboxMessageCreatedEvents = outboxMessageCreatedEvents;
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<OutboxMessage> context,
            CancellationToken cancellationToken)
        {
            _outboxMessageCreatedEvents.Add(OutboxMessageCreatedEvent.Create(_correlationIdProvider.Id,
                context.Entity.Id, _currentUser.Id));
            return Task.CompletedTask;
        }
    }
}