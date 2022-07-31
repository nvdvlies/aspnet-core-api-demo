using System;

namespace Demo.Events.Location;

public class LocationCreatedEvent : Event<LocationCreatedEvent, LocationCreatedEventData>
{
    public static LocationCreatedEvent Create(Guid correlationId, Guid id, Guid createdBy)
    {
        var data = new LocationCreatedEventData { CorrelationId = correlationId, Id = id, CreatedBy = createdBy };
        return new LocationCreatedEvent
        {
            Topic = Topics.Location,
            Subject = $"Location/{data.Id}",
            Data = data,
            DataVersion = data.EventDataVersion,
            CorrelationId = correlationId
        };
    }
}

public class LocationCreatedEventData : IEventData
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
