using MediatR;
using System;

namespace Demo.Events
{
    public interface IEvent : INotification
    {
        string CorrelationId { get; }
        DateTime CreatedOn { get; }
        string DataVersion { get; }
        string Subject { get; }
        Topics Topic { get; }
        string Type { get; }
    }
}