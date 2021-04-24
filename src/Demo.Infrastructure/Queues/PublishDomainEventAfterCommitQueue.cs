using Demo.Domain.Shared.Interfaces;
using System.Collections.Generic;

namespace Demo.Infrastructure.Queues
{
    internal class PublishDomainEventAfterCommitQueue : IPublishDomainEventAfterCommitQueue
    {
        private Queue<IDomainEvent> _queue;

        public PublishDomainEventAfterCommitQueue()
        {
            _queue = new Queue<IDomainEvent>();
        }

        public int Count => _queue.Count;

        public IDomainEvent Dequeue() => _queue.Dequeue();

        public void Enqueue(IDomainEvent item) => _queue.Enqueue(item);
    }
}
