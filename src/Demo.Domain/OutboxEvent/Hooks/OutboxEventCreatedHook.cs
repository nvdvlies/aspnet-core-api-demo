using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxEvent;

namespace Demo.Domain.OutboxEvent.Hooks
{
    internal class OutboxEventCreatedHook : IAfterCreate<OutboxEvent>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ICurrentUser _currentUser;
        private readonly IOutboxEventCreatedEvents _outboxEventCreatedEvents;

        public OutboxEventCreatedHook(
            IOutboxEventCreatedEvents outboxEventCreatedEvents,
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _outboxEventCreatedEvents = outboxEventCreatedEvents;
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<OutboxEvent> context,
            CancellationToken cancellationToken)
        {
            _outboxEventCreatedEvents.Add(OutboxEventCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
                _currentUser.Id));
            return Task.CompletedTask;
        }
    }
}