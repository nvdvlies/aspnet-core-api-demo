using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Users.Messages.SyncAuth0User
{
    public class SyncAuth0UserMessageHandler : IRequestHandler<SyncAuth0UserMessage, Unit>
    {
        private readonly IUserDomainEntity _userDomainEntity;
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;

        public SyncAuth0UserMessageHandler(
            IUserDomainEntity userDomainEntity,
            IAuth0UserManagementClient auth0UserManagementClient
        )
        {
            _userDomainEntity = userDomainEntity;
            _auth0UserManagementClient = auth0UserManagementClient;
        }

        public async Task<Unit> Handle(SyncAuth0UserMessage request, CancellationToken cancellationToken)
        {
            await _userDomainEntity
                .WithOptions(x => x.AsNoTracking = true)
                .GetAsync(request.Data.Id, cancellationToken);

            await _auth0UserManagementClient.SyncAsync(_userDomainEntity.Entity, request.Data.EmailChanged, cancellationToken);

            return Unit.Value;
        }
    }
}
