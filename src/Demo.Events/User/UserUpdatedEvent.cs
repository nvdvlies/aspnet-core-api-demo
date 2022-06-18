using System;

namespace Demo.Events.User
{
    public class UserUpdatedEvent : Event<UserUpdatedEvent, UserUpdatedEventData>
    {
        public static UserUpdatedEvent Create(Guid correlationId, Guid id, Guid updatedBy)
        {
            var data = new UserUpdatedEventData { CorrelationId = correlationId, Id = id, UpdatedBy = updatedBy };
            return new UserUpdatedEvent
            {
                Topic = Topics.User,
                Subject = $"User/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class UserUpdatedEventData : IEventData
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}
