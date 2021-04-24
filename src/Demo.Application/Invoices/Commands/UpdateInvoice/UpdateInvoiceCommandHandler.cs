using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Unit>
    {
        private readonly IInvoiceBusinessComponent _bc;
        private readonly IMapper _mapper;

        public UpdateInvoiceCommandHandler(
            IInvoiceBusinessComponent bc,
            IMapper mapper
        )
        {
            _bc = bc;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _bc.GetAsync(request.Id, cancellationToken);

            _bc.MapFrom(request, _mapper);

            await _bc.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}