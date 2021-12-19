using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Customer.DomainEntity.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly ICustomerDomainEntity _domainEntity;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(
            ICustomerDomainEntity domainEntity, 
            IMapper mapper
        )
        {
            _domainEntity = domainEntity;
            _mapper = mapper;
        }

        public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            await _domainEntity.NewAsync(cancellationToken);

            _domainEntity.MapFrom(request, _mapper);

            await _domainEntity.CreateAsync(cancellationToken);

            return new CreateCustomerResponse
            {
                Id = _domainEntity.EntityId
            };
        }
    }
}