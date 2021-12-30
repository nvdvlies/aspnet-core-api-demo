using System;

namespace Demo.Events.Customer
{
    public class CustomerUpdatedEvent : Event<CustomerUpdatedEventData>
    {
        internal CustomerUpdatedEvent(CustomerUpdatedEventData data) : base(
            Topics.Customer,
            data,
            $"Customer/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static CustomerUpdatedEvent Create(string correlationId, Guid id, Guid updatedBy)
        {
            var data = new CustomerUpdatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                UpdatedBy = updatedBy
            };
            return new CustomerUpdatedEvent(data);
        }
    }

    public class CustomerUpdatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
