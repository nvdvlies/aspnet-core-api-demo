using System;
using System.Text.Json.Serialization;
using Demo.Domain.Shared.Entities;

namespace Demo.Domain.Invoice
{
    public class InvoiceLine : Entity
    {
        [JsonInclude] public int LineNumber { get; internal set; }

        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }

        public Guid? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
