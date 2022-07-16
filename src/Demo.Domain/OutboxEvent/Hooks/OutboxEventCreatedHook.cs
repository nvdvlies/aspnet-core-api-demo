using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxEvent;

namespace Demo.Domain.OutboxEvent.Hooks;

internal class OutboxEventCreatedHook : IAfterCreate<OutboxEvent>
{
    private readonly ICorrelationIdProvider _correlationIdProvider;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IOutboxEventCreatedEvents _outboxEventCreatedEvents;

    public OutboxEventCreatedHook(
        IOutboxEventCreatedEvents outboxEventCreatedEvents,
        ICurrentUserIdProvider currentUserIdProvider,
        ICorrelationIdProvider correlationIdProvider
    )
    {
        _outboxEventCreatedEvents = outboxEventCreatedEvents;
        _currentUserIdProvider = currentUserIdProvider;
        _correlationIdProvider = correlationIdProvider;
    }

    public Task ExecuteAsync(HookType type, IDomainEntityContext<OutboxEvent> context,
        CancellationToken cancellationToken = default)
    {
        _outboxEventCreatedEvents.Add(OutboxEventCreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id,
            _currentUserIdProvider.Id, context.Entity.Type));
        return Task.CompletedTask;
    }
}
