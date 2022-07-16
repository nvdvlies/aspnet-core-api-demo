using System;

namespace Demo.Application.Invoices.Queries.InvoiceLookup.Dtos;

public class InvoiceLookupDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; }
    public Guid CustomerId { get; set; }
}
