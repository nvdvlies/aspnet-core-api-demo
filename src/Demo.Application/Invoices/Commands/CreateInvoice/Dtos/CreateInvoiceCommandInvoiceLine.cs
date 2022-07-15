namespace Demo.Application.Invoices.Commands.CreateInvoice.Dtos
{
    public class CreateInvoiceCommandInvoiceLine
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
