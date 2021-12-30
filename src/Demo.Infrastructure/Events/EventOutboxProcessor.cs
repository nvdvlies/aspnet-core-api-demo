using Demo.Application.Shared.Interfaces;
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
        private readonly IDbCommand<EventOutbox> _dbCommand;
        private readonly IEventPublisher _eventGridPublisher;
        private Queue<EventOutbox> _queue;

        public EventOutboxProcessor(
            ILogger<EventOutboxProcessor> logger,
            IUnitOfWork unitOfWork,
            IDbCommand<EventOutbox> dbCommand,
            IEventPublisher eventGridPublisher)
        {
            _queue = new Queue<EventOutbox>();
            _logger = logger;
            _unitOfWork = unitOfWork;
            _dbCommand = dbCommand;
            _eventGridPublisher = eventGridPublisher;
        }

        public async Task AddToOutboxAsync(Event @event, CancellationToken cancellationToken = default)
        {
            var eventOutbox = new EventOutbox
            {
                Type = @event.Type,
                Event = @event
            };
            eventOutbox.Lock();
            await _dbCommand.InsertAsync(eventOutbox, cancellationToken); 
            _queue.Enqueue(eventOutbox);
        }

        public async Task PublishAllAsync(CancellationToken cancellationToken = default)
        {
            while (_queue.Count > 0)
            {
                var eventOutbox = _queue.Dequeue();
                string lockToken = null;
                try
                {
                    _logger.LogInformation($"Publishing '{eventOutbox.Type}' event");

                    lockToken = await LockAsync(eventOutbox, cancellationToken);
                    await _eventGridPublisher.PublishAsync(eventOutbox.Event, cancellationToken);
                    await MarkAsPublished(eventOutbox, cancellationToken);

                    _logger.LogInformation($"Published '{eventOutbox.Type}' event");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish event");
                    await UnLockAsync(eventOutbox, lockToken, cancellationToken);
                }
            }
        }

        private async Task<string> LockAsync(EventOutbox eventOutbox, CancellationToken cancellationToken)
        {
            var lockToken = eventOutbox.Lock();
            await _dbCommand.UpdateAsync(eventOutbox, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return lockToken;
        }

        private async Task UnLockAsync(EventOutbox eventOutbox, string lockToken, CancellationToken cancellationToken)
        {
            eventOutbox.Unlock(lockToken);
            await _dbCommand.UpdateAsync(eventOutbox, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task MarkAsPublished(EventOutbox eventOutbox, CancellationToken cancellationToken)
        {
            eventOutbox.MarkAsPublished();
            await _dbCommand.UpdateAsync(eventOutbox, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
