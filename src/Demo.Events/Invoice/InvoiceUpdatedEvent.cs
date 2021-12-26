using System;

namespace Demo.Events.Invoice
{
    public class InvoiceUpdatedEvent : BaseEvent<InvoiceUpdatedEventData>
    {
        internal InvoiceUpdatedEvent(InvoiceUpdatedEventData data) : base(
            Topics.Invoice,
            data,
            $"Invoice/{data.Id}",
            data.EventDataVersion,
            data.CorrelationId
            )
        {
        }

        public static InvoiceUpdatedEvent Create(string correlationId, Guid id, Guid updatedBy)
        {
            var data = new InvoiceUpdatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                UpdatedBy = updatedBy
            };
            return new InvoiceUpdatedEvent(data);
        }
    }

    public class InvoiceUpdatedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
