using System;

namespace Demo.Events.Invoice
{
    public class InvoicePaidEvent : Event<InvoicePaidEventData>
    {
        internal InvoicePaidEvent(InvoicePaidEventData data) : base(
            Topics.Invoice,
            data,
            $"Invoice/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static InvoicePaidEvent Create(string correlationId, Guid id)
        {
            var data = new InvoicePaidEventData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new InvoicePaidEvent(data);
        }
    }

    public class InvoicePaidEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
