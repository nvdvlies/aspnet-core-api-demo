using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.User;
using MediatR;

namespace Demo.Application.Users.Messages.CreateAuth0User
{
    public class CreateAuth0UserMessageHandler : IRequestHandler<CreateAuth0UserMessage, Unit>
    {
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;
        private readonly IUserDomainEntity _userDomainEntity;

        public CreateAuth0UserMessageHandler(
            IUserDomainEntity userDomainEntity,
            IAuth0UserManagementClient auth0UserManagementClient
        )
        {
            _userDomainEntity = userDomainEntity;
            _auth0UserManagementClient = auth0UserManagementClient;
        }

        public async Task<Unit> Handle(CreateAuth0UserMessage request, CancellationToken cancellationToken)
        {
            await _userDomainEntity
                .GetAsync(request.Data.Id, cancellationToken);

            var externalId = await _auth0UserManagementClient.CreateAsync(_userDomainEntity.Entity, cancellationToken);

            _userDomainEntity.With(x => x.ExternalId = externalId);

            await _userDomainEntity.UpdateAsync(cancellationToken);

            return Unit.Value;
        }
    }
}