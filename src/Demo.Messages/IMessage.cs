using System;

namespace Demo.Messages
{
    public interface IMessage
    {
        string CorrelationId { get; }
        DateTime CreatedOn { get; }
        string DataVersion { get; }
        string Subject { get; }
        Queues Queue { get; }
        string Type { get; }
    }
}
