using System;

namespace Demo.Events.Invoice
{
    public class InvoiceCreatedEvent : Event<InvoiceCreatedEvent, InvoiceCreatedEventData>
    {
        public static InvoiceCreatedEvent Create(Guid correlationId, Guid id, Guid createdBy)
        {
            var data = new InvoiceCreatedEventData { CorrelationId = correlationId, Id = id, CreatedBy = createdBy };
            return new InvoiceCreatedEvent
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

    public class InvoiceCreatedEventData : IEventData
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}