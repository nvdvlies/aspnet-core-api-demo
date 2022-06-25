using System;
using MediatR;

namespace Demo.Events
{
    public interface IEvent : INotification
    {
        Guid CorrelationId { get; }
        Guid CreatedBy { get; }
        DateTime CreatedOn { get; }
        string DataVersion { get; }
        string Subject { get; }
        Topics Topic { get; }
        string Type { get; }
    }
}
