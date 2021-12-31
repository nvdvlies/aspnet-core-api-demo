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

                _logger.LogInformation("Publishing event of type '{type}'", eventOutbox.Type);

                bool isPublished = false;
                try
                {
                    await _eventGridPublisher.PublishAsync(eventOutbox.Event, cancellationToken);
                    _logger.LogInformation("Published event of type '{type}'", eventOutbox.Type);
                    isPublished = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish event ({type})", eventOutbox.Type);
                    await UnLockAsync(eventOutbox, cancellationToken);
                }

                if (isPublished)
                {
                    try
                    {
                        await MarkAsPublished(eventOutbox, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "Failed to mark published event as published (Type: {type}, ID: {id})", eventOutbox.Type, eventOutbox.Id);
                    }
                }
            }
        }

        private async Task UnLockAsync(EventOutbox eventOutbox, CancellationToken cancellationToken)
        {
            eventOutbox.Unlock();
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
