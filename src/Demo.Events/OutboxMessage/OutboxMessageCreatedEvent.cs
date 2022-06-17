using System;

namespace Demo.Events.OutboxMessage
{
    public class OutboxMessageCreatedEvent : Event<OutboxMessageCreatedEvent, OutboxMessageCreatedEventData>
    {
        public static OutboxMessageCreatedEvent Create(Guid correlationId, Guid id, Guid createdBy)
        {
            var data = new OutboxMessageCreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new OutboxMessageCreatedEvent
            {
                Topic = Topics.OutboxMessage,
                Subject = $"OutboxMessage/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class OutboxMessageCreatedEventData : IEventData
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}