using System;

namespace Demo.Events.Invoice
{
    public class InvoiceCreatedEvent : Event<InvoiceCreatedEvent, InvoiceCreatedEventData>
    {
        internal InvoiceCreatedEvent(InvoiceCreatedEventData data) : base(
            Topics.Invoice,
            data,
            $"Invoice/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static InvoiceCreatedEvent Create(string correlationId, Guid id, Guid createdBy)
        {
            var data = new InvoiceCreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new InvoiceCreatedEvent(data);
        }
    }

    public class InvoiceCreatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
