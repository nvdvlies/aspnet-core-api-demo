using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Demo.Domain.Shared.DomainEntity
{
    internal class DomainEntityContext<T> : IDomainEntityContext<T> where T : IEntity
    {
        private readonly ILogger _logger;
        private readonly IPublishDomainEventAfterCommitQueue _publishDomainEventAfterCommitQueue;
        private readonly IJsonService<T> _jsonService;
        private T _entity;
        private readonly object _entityLock = new object();

        public PerformanceMeasurements PerformanceMeasurements { get; }

        public DomainEntityContext(
            ILogger logger,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            IJsonService<T> jsonService)
        {
            _logger = logger;
            _publishDomainEventAfterCommitQueue = publishDomainEventAfterCommitQueue;
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

        public void PublishDomainEventAfterCommit(IDomainEvent notification)
        {
            _publishDomainEventAfterCommitQueue.Enqueue(notification);
        }

        private T DeepCopy(T entity)
        {
            return _jsonService.FromJson(_jsonService.ToJson(entity));
        }
    }
}
