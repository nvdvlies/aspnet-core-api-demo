using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.User.Interfaces;
using MediatR;

namespace Demo.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserDomainEntity _userDomainEntity;

    public DeleteUserCommandHandler(
        IUserDomainEntity userDomainEntity
    )
    {
        _userDomainEntity = userDomainEntity;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userDomainEntity.GetAsync(request.Id, cancellationToken);

        await _userDomainEntity.DeleteAsync(cancellationToken);

        return Unit.Value;
    }
}
