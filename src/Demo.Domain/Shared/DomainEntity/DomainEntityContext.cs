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
        private readonly Lazy<IEventOutboxProcessor> _eventOutboxProcessor;
        private readonly Lazy<IMessageOutboxProcessor> _messageOutboxProcessor;
        private readonly Lazy<IJsonService<T>> _jsonService;
        private T _entity;
        private readonly object _entityLock = new object();

        public PerformanceMeasurements PerformanceMeasurements { get; }

        public DomainEntityContext(
            ILogger logger,
            Lazy<IEventOutboxProcessor> eventOutboxProcessor,
            Lazy<IMessageOutboxProcessor> messageOutboxProcessor,
            Lazy<IJsonService<T>> jsonService)
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

        public async Task PublishIntegrationEventAsync<E, D>(Event<E, D> @event, CancellationToken cancellationToken) where D : IEventData
        {
            await _eventOutboxProcessor.Value.AddToOutboxAsync(@event, cancellationToken);
        }

        public async Task SendMessageToQueueAsync<M>(Message<M> message, CancellationToken cancellationToken) where M : IMessageData
        {
            await _messageOutboxProcessor.Value.AddToOutboxAsync(message, cancellationToken);
        }

        private T DeepCopy(T entity)
        {
            return _jsonService.Value.FromJson(_jsonService.Value.ToJson(entity));
        }
    }
}
