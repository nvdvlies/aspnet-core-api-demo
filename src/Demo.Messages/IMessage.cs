using System;

namespace Demo.Messages
{
    public interface IMessage<T> : IMessage
    {
        T Data { get; }
    }

    public interface IMessage
    {
        string Type { get; }
        Topics Topic { get; }
        DateTime CreatedOn { get; }
        string Subject { get; }
        string DataVersion { get; }
    }
}
