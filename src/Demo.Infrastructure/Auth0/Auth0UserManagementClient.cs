using Auth0.ManagementApi.Models;
using Demo.Application.Shared.Interfaces;
using Demo.Infrastructure.Settings;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Auth0
{
    internal class Auth0UserManagementClient : IAuth0UserManagementClient
    {
        private readonly EnvironmentSettings _environmentSettings;
        private readonly IAuth0ManagementApiClientProvider _auth0ManagementApiClientCreator;

        public Auth0UserManagementClient(
            EnvironmentSettings environmentSettings,
            IAuth0ManagementApiClientProvider auth0ManagementApiClientCreator
        )
        {
            _environmentSettings = environmentSettings;
            _auth0ManagementApiClientCreator = auth0ManagementApiClientCreator;
        }

        public async Task CreateAsync(Domain.User.User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            //var existingUser = await client.Users.GetAsync(internalUser.Id.ToString(), cancellationToken: cancellationToken); // TODO : make idempotent

            var userCreateRequest = new UserCreateRequest
            {
                Connection = _environmentSettings.Auth0.Connection,
                UserId = internalUser.Id.ToString(),
                Email = internalUser.Email,
                EmailVerified = false,
                Password = string.Concat(internalUser.Id.ToString(), "@" , new Random().Next(9999)),
                FirstName = internalUser.GivenName,
                LastName = $"{internalUser.MiddleName} {internalUser.FamilyName}".Trim(),
                VerifyEmail = false
            };
            var user = await client.Users.CreateAsync(userCreateRequest, cancellationToken);
            var roles = _environmentSettings.Auth0.Auth0RoleMappings
                .Where(x => internalUser.UserRoles.Any(y => y.RoleId == x.InternalRoleId))
                .Select(x => x.Auth0RoleId)
                .ToArray();
            var assignRolesRequest = new AssignRolesRequest
            {
                Roles = roles
            };
            await client.Users.AssignRolesAsync(user.UserId, assignRolesRequest, cancellationToken);
        }

        public async Task<string> GetSetPasswordUrl(Domain.User.User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            var passwordChangeTicketRequest = new PasswordChangeTicketRequest
            {
                UserId = string.Concat("auth0|", internalUser.Id.ToString()),
                MarkEmailAsVerified = true,
                ResultUrl = "http://localhost:4401", // TODO: from config
                IncludeEmailInRedirect = true,
                Ttl = 604800 // one week
            };
            return (await client.Tickets.CreatePasswordChangeTicketAsync(passwordChangeTicketRequest, cancellationToken)).Value;
        }

        public async Task SyncEmailToAuth0Async(Domain.User.User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            var request = new UserUpdateRequest
            {
                Connection = _environmentSettings.Auth0.Connection,
                Email = internalUser.Email,
                EmailVerified = false,
                VerifyEmail = true
            };
            await client.Users.UpdateAsync(internalUser.Id.ToString(), request, cancellationToken);
        }

        public async Task SyncNameToAuth0Async(Domain.User.User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            var request = new UserUpdateRequest
            {
                Connection = _environmentSettings.Auth0.Connection,
                FirstName = internalUser.GivenName,
                LastName = $"{internalUser.MiddleName} {internalUser.FamilyName}".Trim()
            };
            await client.Users.UpdateAsync(internalUser.Id.ToString(), request, cancellationToken);
        }

        public async Task SyncRolesToAuth0Async(Domain.User.User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);

            var roleIdsAssignedToUser = _environmentSettings.Auth0.Auth0RoleMappings
                .Where(x => internalUser.UserRoles.Any(y => y.RoleId == x.InternalRoleId))
                .Select(x => x.Auth0RoleId);

            var roleIdsAssignedToUserInAuth0 = (await client.Users.GetRolesAsync(internalUser.Id.ToString(), null, cancellationToken))
                .Select(x => x.Id);

            var rolesToAdd = roleIdsAssignedToUser
                .Where(internalRoleId => !roleIdsAssignedToUserInAuth0.Any(auth0RoleId => auth0RoleId == internalRoleId))
                .ToArray();
            if (rolesToAdd.Length > 0)
            {
                var assignRolesRequest = new AssignRolesRequest
                {
                    Roles = rolesToAdd
                };
                await client.Users.AssignRolesAsync(internalUser.Id.ToString(), assignRolesRequest, cancellationToken);
            }

            var rolesToRemove = roleIdsAssignedToUserInAuth0
                .Where(auth0RoleId => !roleIdsAssignedToUser.Any(internalRoleId => internalRoleId == auth0RoleId))
                .ToArray();
            if (rolesToRemove.Length > 0)
            {
                var removeRolesRequest = new AssignRolesRequest
                {
                    Roles = rolesToRemove
                };
                await client.Users.RemoveRolesAsync(internalUser.Id.ToString(), removeRolesRequest, cancellationToken);
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            await client.Users.DeleteAsync(id.ToString());
        }
    }
}
