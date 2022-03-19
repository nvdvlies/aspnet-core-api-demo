using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxEvent;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.OutboxEvent.Hooks
{
    internal class OutboxEventCreatedHook : IAfterCreate<OutboxEvent>
    {
        private readonly IOutboxEventCreatedEvents _outboxEventCreatedEvents;
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

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

        public Task ExecuteAsync(HookType type, IDomainEntityContext<OutboxEvent> context, CancellationToken cancellationToken)
        {
            _outboxEventCreatedEvents.Add(OutboxEventCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id));
            return Task.CompletedTask;
        }
    }
}
