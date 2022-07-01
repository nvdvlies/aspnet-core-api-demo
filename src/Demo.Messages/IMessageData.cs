using System;

namespace Demo.Messages
{
    public interface IMessageData
    {
        string MessageDataVersion { get; }
        Guid CorrelationId { get; }
    }
}