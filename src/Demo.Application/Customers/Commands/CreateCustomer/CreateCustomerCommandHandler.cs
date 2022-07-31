using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Customer.Interfaces;
using Demo.Domain.Location.Interfaces;
using MediatR;

namespace Demo.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    private readonly ICustomerDomainEntity _customerDomainEntity;
    private readonly ILocationDomainEntity _locationDomainEntity;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(
        ICustomerDomainEntity customerDomainEntity,
        ILocationDomainEntity locationDomainEntity,
        IMapper mapper
    )
    {
        _customerDomainEntity = customerDomainEntity;
        _locationDomainEntity = locationDomainEntity;
        _mapper = mapper;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        await _customerDomainEntity.NewAsync(cancellationToken);

        if (!string.IsNullOrEmpty(request.Address?.DisplayName))
        {
            await _locationDomainEntity.NewAsync(cancellationToken);
            _locationDomainEntity.MapFrom(request.Address, _mapper);
            await _locationDomainEntity.CreateAsync(cancellationToken);

            _customerDomainEntity.Entity.AddressId = _locationDomainEntity.Entity.Id;
        }

        _customerDomainEntity.MapFrom(request, _mapper);

        await _customerDomainEntity.CreateAsync(cancellationToken);

        return new CreateCustomerResponse { Id = _customerDomainEntity.EntityId };
    }
}
