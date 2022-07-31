using System;

namespace Demo.Events.Location;

public class LocationDeletedEvent : Event<LocationDeletedEvent, LocationDeletedEventData>
{
    public static LocationDeletedEvent Create(Guid correlationId, Guid id, Guid deletedBy)
    {
        var data = new LocationDeletedEventData { CorrelationId = correlationId, Id = id, DeletedBy = deletedBy };
        return new LocationDeletedEvent
        {
            Topic = Topics.Location,
            Subject = $"Location/{data.Id}",
            Data = data,
            DataVersion = data.EventDataVersion,
            CorrelationId = correlationId
        };
    }
}

public class LocationDeletedEventData : IEventData
{
    public Guid Id { get; set; }
    public Guid DeletedBy { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
