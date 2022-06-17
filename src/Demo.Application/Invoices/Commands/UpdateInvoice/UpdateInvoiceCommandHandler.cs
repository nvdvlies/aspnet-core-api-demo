using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Invoice.Interfaces;
using MediatR;

namespace Demo.Application.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Unit>
    {
        private readonly IInvoiceDomainEntity _invoiceDomainEntity;
        private readonly IMapper _mapper;

        public UpdateInvoiceCommandHandler(
            IInvoiceDomainEntity invoiceDomainEntity,
            IMapper mapper
        )
        {
            _invoiceDomainEntity = invoiceDomainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _invoiceDomainEntity.GetAsync(request.Id, cancellationToken);

            _invoiceDomainEntity.MapFrom(request, _mapper);

            await _invoiceDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}