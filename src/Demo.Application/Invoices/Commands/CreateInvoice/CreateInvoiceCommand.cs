using System;
using System.Collections.Generic;
using Demo.Application.Invoices.Commands.CreateInvoice.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommand : ICommand, IRequest<CreateInvoiceResponse>
    {
        public Guid CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int PaymentTerm { get; set; }
        public string OrderReference { get; set; }
        public List<CreateInvoiceCommandInvoiceLine> InvoiceLines { get; set; }
    }
}