namespace Demo.Domain.Shared.Interfaces
{
    public interface IPublishDomainEventAfterCommitQueue
    {
        void Enqueue(IDomainEvent notification);
        int Count { get; }
        IDomainEvent Dequeue();
    }
}
