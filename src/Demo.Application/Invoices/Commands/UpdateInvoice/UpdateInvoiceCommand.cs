using System;
using System.Collections.Generic;
using Demo.Application.Invoices.Commands.UpdateInvoice.Dtos;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }
        // ReSharper disable once InconsistentNaming
        public uint xmin { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int PaymentTerm { get; set; }
        public string OrderReference { get; set; }
        public List<UpdateInvoiceCommandInvoiceLineDto> InvoiceLines { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}
