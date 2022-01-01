using Demo.Domain.Shared.Interfaces;
using Demo.Messages;

namespace Demo.Domain.MessageOutbox.Interfaces
{
    public interface IMessageOutboxDomainEntity : IDomainEntity<MessageOutbox>
    {
        string Type { get; }
        Message Message { get; }
        void Lock(int lockDurationInMinutes = 3);
        void Unlock();
        void MarkAsSent();
    }
}
