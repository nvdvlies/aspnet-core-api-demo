using System;
using System.Collections.Generic;

namespace Demo.Domain.Invoice.BusinessComponent.Models
{
    public class InvoiceToPdfModel
    {
        public int TemplateVersion => 1;
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public List<InvoiceToPdfInvoiceLineModel> InvoiceLines { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(TemplateVersion, InvoiceNumber, InvoiceDate, CustomerName, ContactName, InvoiceLines);
        }
    }

    public class InvoiceToPdfInvoiceLineModel
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string ItemDescription { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Quantity, Description, ItemDescription);
        }
    }
}
