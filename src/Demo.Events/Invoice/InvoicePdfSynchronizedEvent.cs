using System;

namespace Demo.Events.Invoice;

public class InvoicePdfSynchronizedEvent : Event<InvoicePdfSynchronizedEvent, InvoicePdfSynchronizedEventData>
{
    public static InvoicePdfSynchronizedEvent Create(Guid createdBy, Guid correlationId, Guid id)
    {
        var data = new InvoicePdfSynchronizedEventData { CorrelationId = correlationId, Id = id };
        return new InvoicePdfSynchronizedEvent
        {
            Topic = Topics.Invoice,
            Data = data,
            Subject = $"Invoice/{data.Id}",
            DataVersion = data.EventDataVersion,
            CreatedBy = createdBy,
            CorrelationId = data.CorrelationId
        };
    }
}

public class InvoicePdfSynchronizedEventData : IEventData
{
    public Guid Id { get; set; }
    public string EventDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
