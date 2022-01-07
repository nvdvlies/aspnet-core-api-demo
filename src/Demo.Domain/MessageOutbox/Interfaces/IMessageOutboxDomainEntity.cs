using Demo.Domain.Shared.Interfaces;
using Demo.Messages;

namespace Demo.Domain.MessageOutbox.Interfaces
{
    public interface IMessageOutboxDomainEntity : IDomainEntity<MessageOutbox>
    {
        string Type { get; }
        void SetMessage(IMessage message);
        IMessage GetMessage();
        void Lock(int lockDurationInMinutes = 3);
        void Unlock();
        void MarkAsSent();
    }
}
