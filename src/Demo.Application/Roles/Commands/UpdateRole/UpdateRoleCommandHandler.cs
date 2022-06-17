using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Shared.Mappings;
using Demo.Domain.Role.Interfaces;
using MediatR;

namespace Demo.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IRoleDomainEntity _roleDomainEntity;

        public UpdateRoleCommandHandler(
            IRoleDomainEntity roleDomainEntity,
            IMapper mapper
        )
        {
            _roleDomainEntity = roleDomainEntity;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleDomainEntity.GetAsync(request.Id, cancellationToken);

            _roleDomainEntity.MapFrom(request, _mapper);

            await _roleDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}