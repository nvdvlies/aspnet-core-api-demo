using System;

namespace Demo.Events.Invoice
{
    public class InvoiceCreatedEvent : Event<InvoiceCreatedEvent, InvoiceCreatedEventData>
    {
        public static InvoiceCreatedEvent Create(string correlationId, Guid id, string createdBy)
        {
            var data = new InvoiceCreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new InvoiceCreatedEvent
            {
                Topic = Topics.Invoice,
                Data = data,
                Subject = $"Invoice/{data.Id}",
                DataVersion = data.EventDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class InvoiceCreatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
    }
}
