using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.User.Interfaces;
using Demo.Messages.User;
using MediatR;

namespace Demo.Application.Users.Messages.DeleteAuth0User
{
    public class DeleteAuth0UserMessageHandler : IRequestHandler<DeleteAuth0UserMessage, Unit>
    {
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;
        private readonly IUserDomainEntity _userDomainEntity;

        public DeleteAuth0UserMessageHandler(
            IAuth0UserManagementClient auth0UserManagementClient,
            IUserDomainEntity userDomainEntity
        )
        {
            _auth0UserManagementClient = auth0UserManagementClient;
            _userDomainEntity = userDomainEntity;
        }

        public async Task<Unit> Handle(DeleteAuth0UserMessage request, CancellationToken cancellationToken)
        {
            await _userDomainEntity
                .WithOptions(x =>
                {
                    x.AsNoTracking = true;
                    x.IncludeDeleted = true;
                })
                .GetAsync(request.Data.Id, cancellationToken);

            await _auth0UserManagementClient.DeleteAsync(_userDomainEntity.Entity, cancellationToken);

            return Unit.Value;
        }
    }
}