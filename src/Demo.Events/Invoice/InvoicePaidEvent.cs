using System;

namespace Demo.Events.Invoice
{
    public class InvoicePaidEvent : Event<InvoicePaidEvent, InvoicePaidEventData>
    {
        public static InvoicePaidEvent Create(Guid createdBy, Guid correlationId, Guid id)
        {
            var data = new InvoicePaidEventData { CorrelationId = correlationId, Id = id };
            return new InvoicePaidEvent
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

    public class InvoicePaidEventData : IEventData
    {
        public Guid Id { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}
