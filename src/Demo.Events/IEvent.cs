using System;

namespace Demo.Events
{
    public interface IEvent<T> : IEvent
    {
        T Data { get; }
    }

    public interface IEvent
    {
        string Type { get; }
        Topics Topic { get; }
        DateTime CreatedOn { get; }
        string Subject { get; }
        string DataVersion { get; }
    }
}
