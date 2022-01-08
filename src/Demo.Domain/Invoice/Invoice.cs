using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Demo.Domain.Invoice
{
    public partial class Invoice : SoftDeleteEntity, IQueryableEntity
    {
        public Invoice()
        {
            InvoiceLines = new List<InvoiceLine>();
        }

        [JsonInclude]
        public string InvoiceNumber { get; internal set; }
        public Guid CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int PaymentTerm { get; set; }
        public string OrderReference { get; set; }
        [JsonInclude]
        public InvoiceStatus Status { get; internal set; }
        [JsonInclude]
        public bool PdfIsSynced { get; internal set; }
        public string PdfChecksum { get; set; }
        public Customer.Customer Customer { get; set; }
        public List<InvoiceLine> InvoiceLines { get; set; }
    }
}