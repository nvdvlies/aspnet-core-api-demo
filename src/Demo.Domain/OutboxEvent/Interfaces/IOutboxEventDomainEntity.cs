using Demo.Domain.Shared.Interfaces;
using Demo.Events;

namespace Demo.Domain.OutboxEvent.Interfaces
{
    public interface IOutboxEventDomainEntity : IDomainEntity<OutboxEvent>
    {
        string Type { get; }
        void SetEvent(IEvent @event);
        IEvent GetEvent();
        void Lock(int lockDurationInMinutes = 3);
        void Unlock();
        void MarkAsPublished();
    }
}