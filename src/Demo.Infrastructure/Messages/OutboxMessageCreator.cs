using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.OutboxMessage.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages;

namespace Demo.Infrastructure.Messages
{
    internal class OutboxMessageCreator : IOutboxMessageCreator
    {
        private readonly IDomainEntityFactory _domainEntityFactory;

        public OutboxMessageCreator(
            IDomainEntityFactory domainEntityFactory
        )
        {
            _domainEntityFactory = domainEntityFactory;
        }

        public async Task CreateAsync(IMessage message, CancellationToken cancellationToken = default)
        {
            var outboxMessageDomainEntity = _domainEntityFactory.CreateInstance<IOutboxMessageDomainEntity>();
            await outboxMessageDomainEntity.NewAsync(cancellationToken);
            outboxMessageDomainEntity.SetMessage(message);
            outboxMessageDomainEntity.Lock();
            await outboxMessageDomainEntity.CreateAsync(cancellationToken);
        }
    }
}