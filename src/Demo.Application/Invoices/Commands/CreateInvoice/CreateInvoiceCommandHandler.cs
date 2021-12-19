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
        private readonly IInvoiceDomainEntity _domainEntity;
        private readonly IMapper _mapper;

        public CreateInvoiceCommandHandler(
            IInvoiceDomainEntity domainEntity, 
            IMapper mapper
        )
        {
            _domainEntity = domainEntity;
            _mapper = mapper;
        }

        public async Task<CreateInvoiceResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.NewAsync(cancellationToken);

            _domainEntity.MapFrom(request, _mapper);

            await _domainEntity.CreateAsync(cancellationToken);

            return new CreateInvoiceResponse
            {
                Id = _domainEntity.EntityId
            };
        }
    }
}