using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Customer.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly ICustomerDomainEntity _customerDomainEntity;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(
            ICustomerDomainEntity customerDomainEntity, 
            IMapper mapper
        )
        {
            _customerDomainEntity = customerDomainEntity;
            _mapper = mapper;
        }

        public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            await _customerDomainEntity.NewAsync(cancellationToken);

            _customerDomainEntity.MapFrom(request, _mapper);

            await _customerDomainEntity.CreateAsync(cancellationToken);

            return new CreateCustomerResponse
            {
                Id = _customerDomainEntity.EntityId
            };
        }
    }
}