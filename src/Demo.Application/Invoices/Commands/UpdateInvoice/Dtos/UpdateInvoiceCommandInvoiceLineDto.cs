using System;
using Demo.Application.Shared.Interfaces;

namespace Demo.Application.Invoices.Commands.UpdateInvoice.Dtos;

public class UpdateInvoiceCommandInvoiceLineDto : ICreateOrUpdateEntityDto
{
    public int Quantity { get; set; }
    public string Description { get; set; }
    public decimal SellingPrice { get; set; }
    public Guid? Id { get; set; }
}
