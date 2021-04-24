using Demo.Application.Shared.Interfaces;
using System;

namespace Demo.Application.Invoices.Commands.UpdateInvoice.Dtos
{
    public class UpdateInvoiceCommandInvoiceLine : ICreateOrUpdateEntityDto
    {
        public Guid? Id { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
    }
}