using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Customer.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
    {
        private readonly ICustomerDomainEntity _domainEntity;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(
            ICustomerDomainEntity domainEntity, 
            IMapper mapper
        )
        {
            _domainEntity = domainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.GetAsync(request.Id, cancellationToken);

            _domainEntity.MapFrom(request, _mapper);

            await _domainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}