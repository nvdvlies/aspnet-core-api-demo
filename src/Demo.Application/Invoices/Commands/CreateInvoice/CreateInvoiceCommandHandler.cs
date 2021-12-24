using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, CreateInvoiceResponse>
    {
        private readonly IInvoiceDomainEntity _invoiceDomainEntity;
        private readonly IMapper _mapper;

        public CreateInvoiceCommandHandler(
            IInvoiceDomainEntity invoiceDomainEntity, 
            IMapper mapper
        )
        {
            _invoiceDomainEntity = invoiceDomainEntity;
            _mapper = mapper;
        }

        public async Task<CreateInvoiceResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _invoiceDomainEntity.NewAsync(cancellationToken);

            _invoiceDomainEntity.MapFrom(request, _mapper);

            await _invoiceDomainEntity.CreateAsync(cancellationToken);

            return new CreateInvoiceResponse
            {
                Id = _invoiceDomainEntity.EntityId
            };
        }
    }
}