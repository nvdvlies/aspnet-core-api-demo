using MediatR;
using System;
using Demo.Application.Invoices.Commands.PatchInvoice.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace Demo.Application.Invoices.Commands.PatchInvoice
{
    public class PatchInvoiceCommand : IRequest<Unit>
    {
        internal Guid Id { get; set; }
        public JsonPatchDocument<PatchInvoiceCommandInvoice> PatchDocument { get; set; }

        public void SetInvoiceId(Guid id)
        {
            Id = id;
        }
    }
}