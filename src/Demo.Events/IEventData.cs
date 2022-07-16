using System;

namespace Demo.Events;

public interface IEventData
{
    string EventDataVersion { get; }
    Guid CorrelationId { get; }
}
