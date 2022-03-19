using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Users.Messages.SynchronizeInvoicePdf
{
    public class SyncNameToAuth0UserMessageHandler : IRequestHandler<SyncNameToAuth0UserMessage, Unit>
    {
        private readonly IUserDomainEntity _userDomainEntity;
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;

        public SyncNameToAuth0UserMessageHandler(
            IUserDomainEntity userDomainEntity,
            IAuth0UserManagementClient auth0UserManagementClient
        )
        {
            _userDomainEntity = userDomainEntity;
            _auth0UserManagementClient = auth0UserManagementClient;
        }

        public async Task<Unit> Handle(SyncNameToAuth0UserMessage request, CancellationToken cancellationToken)
        {
            await _userDomainEntity
                .WithOptions(x => x.AsNoTracking = true)
                .GetAsync(request.Data.Id, cancellationToken);

            await _auth0UserManagementClient.SyncNameToAuth0Async(_userDomainEntity.Entity, cancellationToken);

            return Unit.Value;
        }
    }
}
