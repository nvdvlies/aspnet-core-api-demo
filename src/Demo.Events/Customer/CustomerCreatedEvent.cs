using System;

namespace Demo.Events.Customer
{
    public class CustomerCreatedEvent : Event<CustomerCreatedEventData>
    {
        internal CustomerCreatedEvent(CustomerCreatedEventData data) : base(
            Topics.Customer,
            data,
            $"Customer/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static CustomerCreatedEvent Create(string correlationId, Guid id, Guid createdBy)
        {
            var data = new CustomerCreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new CustomerCreatedEvent(data);
        }
    }

    public class CustomerCreatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
