using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.User.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserDomainEntity _userDomainEntity;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(
            IUserDomainEntity userDomainEntity,
            IMapper mapper
        )
        {
            _userDomainEntity = userDomainEntity;
            _mapper = mapper;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _userDomainEntity.NewAsync(cancellationToken);

            _userDomainEntity.MapFrom(request, _mapper);

            await _userDomainEntity.CreateAsync(cancellationToken);

            return new CreateUserResponse
            {
                Id = _userDomainEntity.EntityId
            };
        }
    }
}