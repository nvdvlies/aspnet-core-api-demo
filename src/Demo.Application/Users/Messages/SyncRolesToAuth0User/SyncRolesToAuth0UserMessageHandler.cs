using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Users.Messages.SyncRolesToAuth0User
{
    public class SyncRolesToAuth0UserMessageHandler : IRequestHandler<SyncRolesToAuth0UserMessage, Unit>
    {
        private readonly IUserDomainEntity _userDomainEntity;
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;

        public SyncRolesToAuth0UserMessageHandler(
            IUserDomainEntity userDomainEntity,
            IAuth0UserManagementClient auth0UserManagementClient
        )
        {
            _userDomainEntity = userDomainEntity;
            _auth0UserManagementClient = auth0UserManagementClient;
        }

        public async Task<Unit> Handle(SyncRolesToAuth0UserMessage request, CancellationToken cancellationToken)
        {
            await _userDomainEntity
                .WithOptions(x => x.AsNoTracking = true)
                .GetAsync(request.Data.Id, cancellationToken);

            await _auth0UserManagementClient.SyncRolesToAuth0Async(_userDomainEntity.Entity, cancellationToken);

            return Unit.Value;
        }
    }
}
