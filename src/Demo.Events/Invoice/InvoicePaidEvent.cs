using System;

namespace Demo.Events.Invoice
{
    public class InvoicePaidEvent : Event<InvoicePaidEvent, InvoicePaidEventData>
    {
        public static InvoicePaidEvent Create(string correlationId, Guid id)
        {
            var data = new InvoicePaidEventData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new InvoicePaidEvent
            {
                Topic = Topics.Invoice,
                Data = data,
                Subject = $"Invoice/{data.Id}",
                DataVersion = data.EventDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class InvoicePaidEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
