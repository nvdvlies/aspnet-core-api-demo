using System;

namespace Demo.Events.Role
{
    public class RoleDeletedEvent : Event<RoleDeletedEvent, RoleDeletedEventData>
    {
        public static RoleDeletedEvent Create(string correlationId, Guid id, Guid deletedBy)
        {
            var data = new RoleDeletedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                DeletedBy = deletedBy
            };
            return new RoleDeletedEvent
            {
                Topic = Topics.Role,
                Subject = $"Role/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class RoleDeletedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }
    }
}