using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using System.Collections.Generic;

namespace Demo.Infrastructure.QueueOutbox
{
    internal class PublishEventAfterCommitQueue : IPublishEventAfterCommitQueue
    {
        private Queue<IEvent> _queue;

        public PublishEventAfterCommitQueue()
        {
            _queue = new Queue<IEvent>();
        }

        public int Count => _queue.Count;

        public IEvent Dequeue() => _queue.Dequeue();

        public void Enqueue(IEvent @event) => _queue.Enqueue(@event);
    }
}
