using Demo.Domain.Shared.Interfaces;
using Demo.Messages;

namespace Demo.Domain.OutboxMessage.Interfaces;

public interface IOutboxMessageDomainEntity : IDomainEntity<OutboxMessage>
{
    string Type { get; }
    void SetMessage(IMessage message);
    IMessage GetMessage();
    void Lock(int lockDurationInMinutes = 3);
    void Unlock();
    void MarkAsSent();
}
