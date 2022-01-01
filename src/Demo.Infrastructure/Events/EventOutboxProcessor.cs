using Demo.Application.Shared.Interfaces;
using Demo.Domain.EventOutbox.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Events
{
    internal class EventOutboxProcessor : IEventOutboxProcessor
    {
        private readonly ILogger<EventOutboxProcessor> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEntityFactory _domainEntityFactory;
        private readonly IEventPublisher _eventGridPublisher;
        private Queue<IEventOutboxDomainEntity> _queue;

        public EventOutboxProcessor(
            ILogger<EventOutboxProcessor> logger,
            IUnitOfWork unitOfWork,
            IDomainEntityFactory domainEntityFactory,
            IEventPublisher eventGridPublisher)
        {
            _queue = new Queue<IEventOutboxDomainEntity>();
            _logger = logger;
            _unitOfWork = unitOfWork;
            _domainEntityFactory = domainEntityFactory;
            _eventGridPublisher = eventGridPublisher;
        }

        public async Task AddToOutboxAsync(Event @event, CancellationToken cancellationToken = default)
        {
            var eventOutboxDomainEntity = _domainEntityFactory.CreateInstance<IEventOutboxDomainEntity>();
            await eventOutboxDomainEntity.NewAsync(cancellationToken);
            eventOutboxDomainEntity.With(x =>
            {
                x.Type = @event.Type;
                x.Event = @event;
            });
            eventOutboxDomainEntity.Lock();
            await eventOutboxDomainEntity.CreateAsync(cancellationToken);
            _queue.Enqueue(eventOutboxDomainEntity);
        }

        public async Task PublishAllAsync(CancellationToken cancellationToken = default)
        {
            while (_queue.Count > 0)
            {
                var eventOutboxDomainEntity = _queue.Dequeue();

                _logger.LogInformation("Publishing event of type '{type}'", eventOutboxDomainEntity.Type);

                bool isPublished = false;
                try
                {
                    await _eventGridPublisher.PublishAsync(eventOutboxDomainEntity.Event, cancellationToken);
                    _logger.LogInformation("Published event of type '{type}'", eventOutboxDomainEntity.Type);
                    isPublished = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish event of type '{type}' (ID: {id})", eventOutboxDomainEntity.Type, eventOutboxDomainEntity.EntityId);
                    await UnLockAsync(eventOutboxDomainEntity, cancellationToken);
                }

                if (isPublished)
                {
                    try
                    {
                        await UnlockAndMarkAsPublishedAsync(eventOutboxDomainEntity, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "Failed to mark published event of type '{type}' as published (ID: {id})", eventOutboxDomainEntity.Type, eventOutboxDomainEntity.EntityId);
                    }
                }
            }
        }

        private async Task UnLockAsync(IEventOutboxDomainEntity eventOutboxDomainEntity, CancellationToken cancellationToken)
        {
            eventOutboxDomainEntity.Unlock();
            await eventOutboxDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task UnlockAndMarkAsPublishedAsync(IEventOutboxDomainEntity eventOutboxDomainEntity, CancellationToken cancellationToken)
        {
            eventOutboxDomainEntity.Unlock();
            eventOutboxDomainEntity.MarkAsPublished();
            await eventOutboxDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
