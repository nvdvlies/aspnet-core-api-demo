using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using Demo.Messages;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Shared.DomainEntity
{
    internal class DomainEntityContext<T> : IDomainEntityContext<T> where T : IEntity
    {
        private readonly object _entityLock = new();
        private readonly Lazy<IJsonService<T>> _jsonService;
        private readonly ILogger _logger;
        private readonly Lazy<IOutboxEventCreator> _outboxEventCreator;
        private readonly Lazy<IOutboxMessageCreator> _outboxMessageCreator;
        private T _entity;

        public DomainEntityContext(
            ILogger logger,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<T>> jsonService)
        {
            _logger = logger;
            _outboxEventCreator = outboxEventCreator;
            _outboxMessageCreator = outboxMessageCreator;
            _jsonService = jsonService;

            PerformanceMeasurements = new PerformanceMeasurements(logger);
            State = new DomainEntityState();
        }

        public PerformanceMeasurements PerformanceMeasurements { get; }

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

        public async Task AddEventAsync(IEvent @event, CancellationToken cancellationToken)
        {
            await _outboxEventCreator.Value.CreateAsync(@event, cancellationToken);
        }

        public async Task AddMessageAsync(IMessage message, CancellationToken cancellationToken)
        {
            await _outboxMessageCreator.Value.CreateAsync(message, cancellationToken);
        }

        private T DeepCopy(T entity)
        {
            return _jsonService.Value.FromJson(_jsonService.Value.ToJson(entity));
        }
    }
}
