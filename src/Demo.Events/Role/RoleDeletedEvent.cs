using System;

namespace Demo.Events.Role
{
    public class RoleDeletedEvent : Event<RoleDeletedEvent, RoleDeletedEventData>
    {
        public static RoleDeletedEvent Create(Guid correlationId, Guid id, Guid deletedBy)
        {
            var data = new RoleDeletedEventData { CorrelationId = correlationId, Id = id, DeletedBy = deletedBy };
            return new RoleDeletedEvent
            {
                Topic = Topics.Role,
                Subject = $"Role/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CreatedBy = deletedBy,
                CorrelationId = correlationId
            };
        }
    }

    public class RoleDeletedEventData : IEventData
    {
        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}
