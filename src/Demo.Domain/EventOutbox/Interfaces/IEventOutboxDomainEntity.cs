using Demo.Domain.Shared.Interfaces;
using Demo.Events;

namespace Demo.Domain.EventOutbox.Interfaces
{
    public interface IEventOutboxDomainEntity : IDomainEntity<EventOutbox>
    {
        string Type { get; }
        void SetEvent(IEvent @event);
        IEvent GetEvent();
        void Lock(int lockDurationInMinutes = 3);
        void Unlock();
        void MarkAsPublished();
    }
}
