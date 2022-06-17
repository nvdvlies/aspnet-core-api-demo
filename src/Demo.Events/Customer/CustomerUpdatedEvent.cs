using System;

namespace Demo.Events.Customer
{
    public class CustomerUpdatedEvent : Event<CustomerUpdatedEvent, CustomerUpdatedEventData>
    {
        public static CustomerUpdatedEvent Create(Guid correlationId, Guid id, Guid updatedBy)
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
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}