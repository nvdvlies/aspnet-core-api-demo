using System;

namespace Demo.Events.Customer
{
    public class CustomerCreatedEvent : Event<CustomerCreatedEvent, CustomerCreatedEventData>
    {
        public static CustomerCreatedEvent Create(string correlationId, Guid id, string createdBy)
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
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
    }
}
