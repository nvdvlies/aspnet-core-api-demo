using Demo.Application.Shared.Interfaces;
using Demo.Domain.OutboxEvent.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Events
{
    internal class OutboxEventPublisher : IOutboxEventPublisher
    {
        private readonly ILogger<OutboxEventPublisher> _logger;
        private readonly IDomainEntityFactory _domainEntityFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUnitOfWork _unitOfWork;

        public OutboxEventPublisher(
            ILogger<OutboxEventPublisher> logger,
            IDomainEntityFactory domainEntityFactory,
            IEventPublisher eventPublisher,
            IUnitOfWork unitOfWork
        )
        {
            _logger = logger;
            _domainEntityFactory = domainEntityFactory;
            _eventPublisher = eventPublisher;
            _unitOfWork = unitOfWork;
        }

        public async Task PublishAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Publishing OutboxEvent entity with id '{id}'", id);

            var outboxEventDomainEntity = _domainEntityFactory.CreateInstance<IOutboxEventDomainEntity>();
            await outboxEventDomainEntity.GetAsync(id, cancellationToken);

            _logger.LogInformation("Publishing event of type '{type}'", outboxEventDomainEntity.Type);

            bool isPublished = false;
            try
            {
                var @event = outboxEventDomainEntity.GetEvent();
                await _eventPublisher.PublishAsync(@event, cancellationToken);
                _logger.LogInformation("Published event of type '{type}'", outboxEventDomainEntity.Type);
                isPublished = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish event of type '{type}' (ID: {id})", outboxEventDomainEntity.Type, outboxEventDomainEntity.EntityId);
                await UnLockAsync(outboxEventDomainEntity, cancellationToken);
            }

            if (isPublished)
            {
                try
                {
                    await UnlockAndMarkAsPublishedAsync(outboxEventDomainEntity, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Failed to mark published event of type '{type}' as published (ID: {id})", outboxEventDomainEntity.Type, outboxEventDomainEntity.EntityId);
                }
            }
        }

        private async Task UnLockAsync(IOutboxEventDomainEntity outboxEventDomainEntity, CancellationToken cancellationToken)
        {
            outboxEventDomainEntity.Unlock();
            await outboxEventDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task UnlockAndMarkAsPublishedAsync(IOutboxEventDomainEntity outboxEventDomainEntity, CancellationToken cancellationToken)
        {
            outboxEventDomainEntity.Unlock();
            outboxEventDomainEntity.MarkAsPublished();
            await outboxEventDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
