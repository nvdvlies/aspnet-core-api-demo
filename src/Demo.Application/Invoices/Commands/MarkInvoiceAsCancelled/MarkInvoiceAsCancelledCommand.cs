using MediatR;
using System;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsCancelled
{
    public class MarkInvoiceAsCancelledCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}