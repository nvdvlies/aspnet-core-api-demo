using MediatR;
using System;

namespace Demo.Application.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}