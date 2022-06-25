using System;

namespace Demo.Messages.Invoice
{
    public class SynchronizeInvoicePdfMessage : Message<SynchronizeInvoicePdfMessage, SynchronizeInvoicePdfMessageData>
    {
        public static SynchronizeInvoicePdfMessage Create(Guid createdBy, Guid correlationId, Guid id)
        {
            var data = new SynchronizeInvoicePdfMessageData { CorrelationId = correlationId, Id = id };
            return new SynchronizeInvoicePdfMessage
            {
                Queue = Queues.SynchronizeInvoicePdf,
                Data = data,
                Subject = $"Invoice/{data.Id}",
                DataVersion = data.MessageDataVersion,
                CreatedBy = createdBy,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class SynchronizeInvoicePdfMessageData : IMessageData
    {
        public Guid Id { get; set; }
        public string MessageDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}
