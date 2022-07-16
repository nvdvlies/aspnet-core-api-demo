using System;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.CopyInvoice;

public class CopyInvoiceCommand : ICommand, IRequest<CopyInvoiceResponse>
{
    internal Guid Id { get; set; }

    public void SetInvoiceId(Guid id)
    {
        Id = id;
    }
}
