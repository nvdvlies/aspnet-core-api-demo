using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IRoleDomainEntity _roleDomainEntity;

    public DeleteRoleCommandHandler(
        IRoleDomainEntity roleDomainEntity
    )
    {
        _roleDomainEntity = roleDomainEntity;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await _roleDomainEntity.GetAsync(request.Id, cancellationToken);

        await _roleDomainEntity.DeleteAsync(cancellationToken);

        return Unit.Value;
    }
}
