using Demo.Domain.Shared.Entities;
using System;

namespace Demo.Domain.Invoice
{
    public partial class InvoiceLine : Entity
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }

        public Guid? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
