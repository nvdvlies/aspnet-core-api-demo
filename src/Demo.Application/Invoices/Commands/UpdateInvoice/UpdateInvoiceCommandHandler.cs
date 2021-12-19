using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Invoice.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _domainEntity;
        private readonly IMapper _mapper;

        public UpdateInvoiceCommandHandler(
            IInvoiceDomainEntity domainEntity,
            IMapper mapper
        )
        {
            _domainEntity = domainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(request.Id, cancellationToken);

            _domainEntity.MapFrom(request, _mapper);

            await _domainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}