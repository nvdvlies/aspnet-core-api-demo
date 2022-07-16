using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Role.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
{
    private readonly IMapper _mapper;
    private readonly IRoleDomainEntity _roleDomainEntity;

    public CreateRoleCommandHandler(
        IRoleDomainEntity roleDomainEntity,
        IMapper mapper
    )
    {
        _roleDomainEntity = roleDomainEntity;
        _mapper = mapper;
    }

    public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        await _roleDomainEntity.NewAsync(cancellationToken);

        _roleDomainEntity.MapFrom(request, _mapper);

        await _roleDomainEntity.CreateAsync(cancellationToken);

        return new CreateRoleResponse { Id = _roleDomainEntity.EntityId };
    }
}
