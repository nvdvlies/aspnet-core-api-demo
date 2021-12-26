using Demo.Events;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IPublishEventAfterCommitQueue
    {
        void Enqueue(IEvent @event);
        int Count { get; }
        IEvent Dequeue();
    }
}
