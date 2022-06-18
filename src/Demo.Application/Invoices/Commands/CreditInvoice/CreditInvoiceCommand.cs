using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.CreditInvoice
{
    public class CreditInvoiceCommand : ICommand, IRequest<CreditInvoiceResponse>
    {
        internal Guid Id { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}
