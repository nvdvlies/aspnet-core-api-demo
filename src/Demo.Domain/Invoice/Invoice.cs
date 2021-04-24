using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Demo.Domain.Invoice
{
    public partial class Invoice : SoftDeleteEntity, IQueryableEntity
    {
        public string InvoiceNumber { get; internal set; }
        public Guid CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int PaymentTerm { get; set; }
        public string OrderReference { get; set; }
        public InvoiceStatus Status { get; internal set; }
        public bool PdfIsSynced { get; internal set; }
        public int PdfHashcode { get; set; }
        public Customer.Customer Customer { get; set; }
        public List<InvoiceLine> InvoiceLines { get; set; }
    }
}