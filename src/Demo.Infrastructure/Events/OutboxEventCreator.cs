using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.OutboxEvent.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;

namespace Demo.Infrastructure.Events;

internal class OutboxEventCreator : IOutboxEventCreator
{
    private readonly IDomainEntityFactory _domainEntityFactory;

    public OutboxEventCreator(
        IDomainEntityFactory domainEntityFactory
    )
    {
        _domainEntityFactory = domainEntityFactory;
    }

    public async Task CreateAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        var outboxEventDomainEntity = _domainEntityFactory.CreateInstance<IOutboxEventDomainEntity>();
        await outboxEventDomainEntity.NewAsync(cancellationToken);
        outboxEventDomainEntity.SetEvent(@event);
        outboxEventDomainEntity.Lock();
        await outboxEventDomainEntity.CreateAsync(cancellationToken);
    }
}
