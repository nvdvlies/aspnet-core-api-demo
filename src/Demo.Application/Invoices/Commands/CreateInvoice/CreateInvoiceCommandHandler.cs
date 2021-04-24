using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Invoice.BusinessComponent.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, CreateInvoiceResponse>
    {
        private readonly IInvoiceBusinessComponent _bc;
        private readonly IMapper _mapper;

        public CreateInvoiceCommandHandler(
            IInvoiceBusinessComponent bc, 
            IMapper mapper
        )
        {
            _bc = bc;
            _mapper = mapper;
        }

        public async Task<CreateInvoiceResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _bc.NewAsync(cancellationToken);

            _bc.MapFrom(request, _mapper);

            await _bc.CreateAsync(cancellationToken);

            return new CreateInvoiceResponse
            {
                Id = _bc.EntityId
            };
        }
    }
}