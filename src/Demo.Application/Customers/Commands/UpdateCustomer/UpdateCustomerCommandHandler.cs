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
        private readonly ICustomerDomainEntity _customerDomainEntity;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(
            ICustomerDomainEntity customerDomainEntity, 
            IMapper mapper
        )
        {
            _customerDomainEntity = customerDomainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            await _customerDomainEntity.GetAsync(request.Id, cancellationToken);

            _customerDomainEntity.MapFrom(request, _mapper);

            await _customerDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}