using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Messages.User;
using MediatR;

namespace Demo.Application.Users.Messages.DeleteAuth0User
{
    public class DeleteAuth0UserMessageHandler : IRequestHandler<DeleteAuth0UserMessage, Unit>
    {
        private readonly IAuth0UserManagementClient _auth0UserManagementClient;

        public DeleteAuth0UserMessageHandler(
            IAuth0UserManagementClient auth0UserManagementClient
        )
        {
            _auth0UserManagementClient = auth0UserManagementClient;
        }

        public async Task<Unit> Handle(DeleteAuth0UserMessage request, CancellationToken cancellationToken)
        {
            await _auth0UserManagementClient.DeleteAsync(request.Data.Id, cancellationToken);

            return Unit.Value;
        }
    }
}
