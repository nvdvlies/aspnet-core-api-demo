using System;

namespace Demo.Events.Location;

public class LocationUpdatedEvent : Event<LocationUpdatedEvent, LocationUpdatedEventData>
{
    public static LocationUpdatedEvent Create(Guid correlationId, Guid id, Guid updatedBy)
    {
        var data = new LocationUpdatedEventData { CorrelationId = correlationId, Id = id, UpdatedBy = updatedBy };
        return new LocationUpdatedEvent
        {
            Topic = Topics.Location,
            Subject = $"Location/{data.Id}",
            Data = data,
            DataVersion = data.EventDataVersion,
            CorrelationId = correlationId
        };
    }
}

public class LocationUpdatedEventData : IEventData
{
    public Guid Id { get; set; }
    public Guid UpdatedBy { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
