using System;

namespace Demo.Events.Customer;

public class CustomerDeletedEvent : Event<CustomerDeletedEvent, CustomerDeletedEventData>
{
    public static CustomerDeletedEvent Create(Guid correlationId, Guid id, Guid deletedBy)
    {
        var data = new CustomerDeletedEventData { CorrelationId = correlationId, Id = id, DeletedBy = deletedBy };
        return new CustomerDeletedEvent
        {
            Topic = Topics.Customer,
            Subject = $"Customer/{data.Id}",
            Data = data,
            DataVersion = data.EventDataVersion,
            CreatedBy = deletedBy,
            CorrelationId = correlationId
        };
    }
}

public class CustomerDeletedEventData : IEventData
{
    public Guid Id { get; set; }
    public Guid DeletedBy { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
