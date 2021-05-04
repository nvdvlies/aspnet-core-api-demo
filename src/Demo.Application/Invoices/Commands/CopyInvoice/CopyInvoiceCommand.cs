using MediatR;
using System;

namespace Demo.Application.Invoices.Commands.CopyInvoice
{
    public class CopyInvoiceCommand : IRequest<CopyInvoiceResponse>
    {
        internal Guid Id { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}