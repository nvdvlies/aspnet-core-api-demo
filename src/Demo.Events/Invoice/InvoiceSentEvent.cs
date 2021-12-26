using System;

namespace Demo.Events.Invoice
{
    public class InvoiceSentEvent : BaseEvent<InvoiceSentEventData>
    {
        internal InvoiceSentEvent(InvoiceSentEventData data) : base(
            Topics.Invoice,
            data,
            $"Invoice/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static InvoiceSentEvent Create(string correlationId, Guid id)
        {
            var data = new InvoiceSentEventData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new InvoiceSentEvent(data);
        }
    }

    public class InvoiceSentEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
