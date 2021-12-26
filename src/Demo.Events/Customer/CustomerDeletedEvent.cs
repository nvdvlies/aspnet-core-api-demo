using System;

namespace Demo.Events.Customer
{
    public class CustomerDeletedEvent : BaseEvent<CustomerDeletedEventData>
    {
        internal CustomerDeletedEvent(CustomerDeletedEventData data) : base(
            Topics.Customer,
            data,
            $"Customer/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static CustomerDeletedEvent Create(string correlationId, Guid id, Guid deletedBy)
        {
            var data = new CustomerDeletedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                DeletedBy = deletedBy
            };
            return new CustomerDeletedEvent(data);
        }
    }

    public class CustomerDeletedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }
    }
}
