using Demo.Application.Shared.Dtos;

namespace Demo.Application.Invoices.Queries.GetInvoiceById.Dtos;

public class InvoiceLineDto : EntityDto
{
    public int LineNumber { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
    public decimal SellingPrice { get; set; }
}
