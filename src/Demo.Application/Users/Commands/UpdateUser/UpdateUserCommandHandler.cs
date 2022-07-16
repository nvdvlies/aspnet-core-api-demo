using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.User.Interfaces;
using MediatR;

namespace Demo.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IUserDomainEntity _userDomainEntity;

    public UpdateUserCommandHandler(
        IUserDomainEntity userDomainEntity,
        IMapper mapper
    )
    {
        _userDomainEntity = userDomainEntity;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await _userDomainEntity.GetAsync(request.Id, cancellationToken);

        _userDomainEntity.MapFrom(request, _mapper);

        await _userDomainEntity.UpdateAsync(cancellationToken);

        return Unit.Value;
    }
}
