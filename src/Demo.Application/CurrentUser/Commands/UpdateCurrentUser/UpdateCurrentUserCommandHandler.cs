using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using MediatR;

namespace Demo.Application.CurrentUser.Commands.UpdateCurrentUser
{
    public class UpdateCurrentUserCommandHandler : IRequestHandler<UpdateCurrentUserCommand, Unit>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IMapper _mapper;
        private readonly IUserDomainEntity _userDomainEntity;

        public UpdateCurrentUserCommandHandler(
            IUserDomainEntity userDomainEntity,
            ICurrentUserIdProvider currentUserIdProvider,
            IMapper mapper
        )
        {
            _userDomainEntity = userDomainEntity;
            _currentUserIdProvider = currentUserIdProvider;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
        {
            await _userDomainEntity.GetAsync(_currentUserIdProvider.Id, cancellationToken);

            _userDomainEntity.MapFrom(request, _mapper);

            await _userDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}