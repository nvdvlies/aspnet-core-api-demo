using System;

namespace Demo.Events.Customer
{
    public class CustomerCreatedEvent : Event<CustomerCreatedEvent, CustomerCreatedEventData>
    {
        public static CustomerCreatedEvent Create(Guid correlationId, Guid id, Guid createdBy)
        {
            var data = new CustomerCreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new CustomerCreatedEvent
            {
                Topic = Topics.Customer,
                Subject = $"Customer/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class CustomerCreatedEventData : IEventData
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}