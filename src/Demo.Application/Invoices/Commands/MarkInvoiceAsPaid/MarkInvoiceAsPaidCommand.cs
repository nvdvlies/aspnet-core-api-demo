using Demo.Application.Shared.Interfaces;
using MediatR;
using System;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsPaid
{
    public class MarkInvoiceAsPaidCommand : ICommand, IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}