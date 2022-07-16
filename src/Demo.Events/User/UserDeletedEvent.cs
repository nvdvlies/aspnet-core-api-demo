using System;

namespace Demo.Events.User;

public class UserDeletedEvent : Event<UserDeletedEvent, UserDeletedEventData>
{
    public static UserDeletedEvent Create(Guid correlationId, Guid id, Guid deletedBy)
    {
        var data = new UserDeletedEventData { CorrelationId = correlationId, Id = id, DeletedBy = deletedBy };
        return new UserDeletedEvent
        {
            Topic = Topics.User,
            Subject = $"User/{data.Id}",
            Data = data,
            DataVersion = data.EventDataVersion,
            CreatedBy = deletedBy,
            CorrelationId = correlationId
        };
    }
}

public class UserDeletedEventData : IEventData
{
    public Guid Id { get; set; }
    public Guid DeletedBy { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
