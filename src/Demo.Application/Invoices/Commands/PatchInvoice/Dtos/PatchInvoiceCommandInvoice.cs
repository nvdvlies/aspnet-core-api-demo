using System;
using System.Collections.Generic;

namespace Demo.Application.Invoices.Commands.PatchInvoice.Dtos
{
    public class PatchInvoiceCommandInvoice
    {
        public Guid CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int PaymentTerm { get; set; }
        public string OrderReference { get; set; }
        public List<PatchInvoiceCommandInvoiceLine> InvoiceLines { get; set; }
    }
}