using System;

namespace Demo.Events.Customer
{
    public class CustomerUpdatedEvent : Event<CustomerUpdatedEvent, CustomerUpdatedEventData>
    {
        public static CustomerUpdatedEvent Create(string correlationId, Guid id, string updatedBy)
        {
            var data = new CustomerUpdatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                UpdatedBy = updatedBy
            };
            return new CustomerUpdatedEvent
            {
                Topic = Topics.Customer,
                Subject = $"Customer/{data.Id}",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class CustomerUpdatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public string UpdatedBy { get; set; }
    }
}
