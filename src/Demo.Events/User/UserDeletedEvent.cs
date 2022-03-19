using System;

namespace Demo.Events.User
{
    public class UserDeletedEvent : Event<UserDeletedEvent, UserDeletedEventData>
    {
        public static UserDeletedEvent Create(string correlationId, Guid id, Guid deletedBy)
        {
            var data = new UserDeletedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                DeletedBy = deletedBy
            };
            return new UserDeletedEvent
            {
                Topic = Topics.User,
                Subject = $"User/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class UserDeletedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }
    }
}