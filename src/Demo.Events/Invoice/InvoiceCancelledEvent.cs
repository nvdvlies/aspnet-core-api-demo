using System;

namespace Demo.Events.Invoice
{
    public class InvoiceCancelledEvent : Event<InvoiceCancelledEventData>
    {
        internal InvoiceCancelledEvent(InvoiceCancelledEventData data) : base(
            Topics.Invoice,
            data,
            $"Invoice/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static InvoiceCancelledEvent Create(string correlationId, Guid id)
        {
            var data = new InvoiceCancelledEventData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new InvoiceCancelledEvent(data);
        }
    }

    public class InvoiceCancelledEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
