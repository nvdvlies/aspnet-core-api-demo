using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Users.Messages.SynchronizeInvoicePdf
{
    public class CreateAuth0UserMessageHandler : IRequestHandler<CreateAuth0UserMessage, Unit>
    {
        private readonly IUserDomainEntity _userDomainEntity;
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;

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
                .WithOptions(x => x.AsNoTracking = true)
                .GetAsync(request.Data.Id, cancellationToken);

            await _auth0UserManagementClient.CreateAsync(_userDomainEntity.Entity, cancellationToken);

            return Unit.Value;
        }
    }
}
