using System;

namespace Demo.Events.Role
{
    public class RoleUpdatedEvent : Event<RoleUpdatedEvent, RoleUpdatedEventData>
    {
        public static RoleUpdatedEvent Create(Guid correlationId, Guid id, Guid updatedBy)
        {
            var data = new RoleUpdatedEventData { CorrelationId = correlationId, Id = id, UpdatedBy = updatedBy };
            return new RoleUpdatedEvent
            {
                Topic = Topics.Role,
                Subject = $"Role/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CreatedBy = updatedBy,
                CorrelationId = correlationId
            };
        }
    }

    public class RoleUpdatedEventData : IEventData
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}