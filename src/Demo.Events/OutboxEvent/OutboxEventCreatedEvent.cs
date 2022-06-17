using System;

namespace Demo.Events.OutboxEvent
{
    public class OutboxEventCreatedEvent : Event<OutboxEventCreatedEvent, OutboxEventCreatedEventData>
    {
        public static OutboxEventCreatedEvent Create(Guid correlationId, Guid id, Guid createdBy)
        {
            var data = new OutboxEventCreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new OutboxEventCreatedEvent
            {
                Topic = Topics.OutboxEvent,
                Subject = $"OutboxEvent/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class OutboxEventCreatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
