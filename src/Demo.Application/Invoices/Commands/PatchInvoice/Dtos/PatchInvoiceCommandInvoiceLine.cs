using System;

namespace Demo.Application.Invoices.Commands.PatchInvoice.Dtos
{
    public class PatchInvoiceCommandInvoiceLine
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
    }
}