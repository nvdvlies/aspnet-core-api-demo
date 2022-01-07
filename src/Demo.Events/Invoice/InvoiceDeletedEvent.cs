using System;

namespace Demo.Events.Invoice
{
    public class InvoiceDeletedEvent : Event<InvoiceDeletedEvent, InvoiceDeletedEventData>
    {
        public static InvoiceDeletedEvent Create(string correlationId, Guid id, Guid deletedBy)
        {
            var data = new InvoiceDeletedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                DeletedBy = deletedBy
            };
            return new InvoiceDeletedEvent
            {
                Topic = Topics.Invoice,
                Data = data,
                Subject = $"Invoice/{data.Id}",
                DataVersion = data.EventDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class InvoiceDeletedEventData : IEventData
    {
        public string EventDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }
    }
}
