using System;

namespace Demo.Events.Invoice
{
    public class InvoicePdfSynchronizedEvent : BaseEvent<InvoicePdfSynchronizedEventData>
    {
        internal InvoicePdfSynchronizedEvent(InvoicePdfSynchronizedEventData data) : base(
            Topics.Invoice,
            data,
            $"Invoice/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static InvoicePdfSynchronizedEvent Create(string correlationId, Guid id)
        {
            var data = new InvoicePdfSynchronizedEventData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new InvoicePdfSynchronizedEvent(data);
        }
    }

    public class InvoicePdfSynchronizedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
