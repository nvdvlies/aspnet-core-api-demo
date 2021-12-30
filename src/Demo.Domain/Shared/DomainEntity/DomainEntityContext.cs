using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using Demo.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.DomainEntity
{
    internal class DomainEntityContext<T> : IDomainEntityContext<T> where T : IEntity
    {
        private readonly ILogger _logger;
        private readonly IEventOutboxProcessor _eventOutboxProcessor;
        private readonly IMessageOutboxProcessor _messageOutboxProcessor;
        private readonly IJsonService<T> _jsonService;
        private T _entity;
        private readonly object _entityLock = new object();

        public PerformanceMeasurements PerformanceMeasurements { get; }

        public DomainEntityContext(
            ILogger logger,
            IEventOutboxProcessor eventOutboxProcessor,
            IMessageOutboxProcessor messageOutboxProcessor,
            IJsonService<T> jsonService)
        {
            _logger = logger;
            _eventOutboxProcessor = eventOutboxProcessor;
            _messageOutboxProcessor = messageOutboxProcessor;
            _jsonService = jsonService;

            PerformanceMeasurements = new PerformanceMeasurements(logger);
            State = new DomainEntityState();
        }

        public T Entity
        {
            get => _entity;
            set
            {
                lock (_entityLock)
                {
                    _entity = value;
                    Pristine = DeepCopy(value);
                }
            }
        }
        public T Pristine { get; private set; }
        public EditMode EditMode { get; set; }
        public IDomainEntityState State { get; }
        public bool IsNewEntity => Entity?.Id == Guid.Empty;

        public async Task PublishIntegrationEventAsync<E>(Event<E> @event, CancellationToken cancellationToken) where E : IEventData
        {
            await _eventOutboxProcessor.AddToOutboxAsync(@event, cancellationToken);
        }

        public async Task SendMessageToQueueAsync<M>(Message<M> message, CancellationToken cancellationToken) where M : IMessageData
        {
            await _messageOutboxProcessor.AddToOutboxAsync(message, cancellationToken);
        }

        private T DeepCopy(T entity)
        {
            return _jsonService.FromJson(_jsonService.ToJson(entity));
        }
    }
}
