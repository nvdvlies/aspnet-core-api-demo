using AutoMapper;
using Demo.Application.Invoices.Commands.PatchInvoice.Dtos;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.PatchInvoice
{
    public class PatchInvoiceCommandHandler : IRequestHandler<PatchInvoiceCommand, Unit>
    {
        private readonly IInvoiceBusinessComponent _bc;
        private readonly IMapper _mapper;

        public PatchInvoiceCommandHandler(
            IInvoiceBusinessComponent bc,
            IMapper mapper
        )
        {
            _bc = bc;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(PatchInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            _bc.PatchFrom(request.PatchDocument, _mapper);

            await _bc.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}