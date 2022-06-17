using System;

namespace Demo.Events.Invoice
{
    public class InvoiceSentEvent : Event<InvoiceSentEvent, InvoiceSentEventData>
    {
        public static InvoiceSentEvent Create(Guid correlationId, Guid id)
        {
            var data = new InvoiceSentEventData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new InvoiceSentEvent
            {
                Topic = Topics.Invoice,
                Data = data,
                Subject = $"Invoice/{data.Id}",
                DataVersion = data.EventDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class InvoiceSentEventData : IEventData
    {
        public Guid Id { get; set; }
        public string EventDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}