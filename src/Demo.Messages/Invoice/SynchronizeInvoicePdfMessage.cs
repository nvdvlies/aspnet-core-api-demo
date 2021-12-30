using System;

namespace Demo.Messages.Invoice
{
    public class SynchronizeInvoicePdfMessage : Message<SynchronizeInvoicePdfMessageData>
    {
        public SynchronizeInvoicePdfMessage(SynchronizeInvoicePdfMessageData data) : base(
            Queues.SynchronizeInvoicePdf,
            data,
            $"Invoice/{data.Id}",
            data.MessageDataVersion,
            data.CorrelationId
            )
        {
        }

        public static SynchronizeInvoicePdfMessage Create(string correlationId, Guid id)
        {
            var data = new SynchronizeInvoicePdfMessageData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new SynchronizeInvoicePdfMessage(data);
        }
    }

    public class SynchronizeInvoicePdfMessageData : IMessageData
    {
        public string MessageDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
