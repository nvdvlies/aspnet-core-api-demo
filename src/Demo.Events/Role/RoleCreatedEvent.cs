using System;

namespace Demo.Events.Role;

public class RoleCreatedEvent : Event<RoleCreatedEvent, RoleCreatedEventData>
{
    public static RoleCreatedEvent Create(Guid correlationId, Guid id, Guid createdBy)
    {
        var data = new RoleCreatedEventData { CorrelationId = correlationId, Id = id, CreatedBy = createdBy };
        return new RoleCreatedEvent
        {
            Topic = Topics.Role,
            Subject = $"Role/{data.Id}",
            Data = data,
            DataVersion = data.EventDataVersion,
            CreatedBy = createdBy,
            CorrelationId = correlationId
        };
    }
}

public class RoleCreatedEventData : IEventData
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
