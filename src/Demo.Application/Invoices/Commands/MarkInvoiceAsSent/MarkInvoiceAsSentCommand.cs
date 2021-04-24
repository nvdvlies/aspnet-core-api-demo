using MediatR;
using System;

namespace Demo.Application.Invoices.Commands.MarkInvoiceAsSent
{
    public class MarkInvoiceAsSentCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}