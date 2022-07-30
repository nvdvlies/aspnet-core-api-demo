﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Customer.Interfaces;
using Demo.Domain.Location.Interfaces;
using MediatR;

namespace Demo.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly ICustomerDomainEntity _customerDomainEntity;
    private readonly ILocationDomainEntity _locationDomainEntity;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(
        ICustomerDomainEntity customerDomainEntity,
        ILocationDomainEntity locationDomainEntity,
        IMapper mapper
    )
    {
        _customerDomainEntity = customerDomainEntity;
        _locationDomainEntity = locationDomainEntity;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        await _customerDomainEntity.GetAsync(request.Id, cancellationToken);

        if (request.AddressId.HasValue)
        {
            if (_customerDomainEntity.Entity.AddressId.HasValue)
            {
                await _locationDomainEntity.GetAsync(request.AddressId.Value, cancellationToken);
                _locationDomainEntity.MapFrom(request.Address, _mapper);
                await _locationDomainEntity.UpdateAsync(cancellationToken);
            }
            else
            {
                await _locationDomainEntity.NewAsync(cancellationToken);
                _locationDomainEntity.Entity.Id = request.AddressId.Value;
                _locationDomainEntity.MapFrom(request.Address, _mapper);
                await _locationDomainEntity.CreateAsync(cancellationToken);
            }
        }

        _customerDomainEntity.MapFrom(request, _mapper);

        await _customerDomainEntity.UpdateAsync(cancellationToken);

        return Unit.Value;
    }
}
