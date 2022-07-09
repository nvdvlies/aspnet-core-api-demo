using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi.Models;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Settings;
using User = Demo.Domain.User.User;

namespace Demo.Infrastructure.Auth0
{
    internal class Auth0UserManagementClient : IAuth0UserManagementClient
    {
        private readonly IAuth0ManagementApiClientProvider _auth0ManagementApiClientCreator;
        private readonly EnvironmentSettings _environmentSettings;
        private readonly IRolesProvider _rolesProvider;

        public Auth0UserManagementClient(
            EnvironmentSettings environmentSettings,
            IAuth0ManagementApiClientProvider auth0ManagementApiClientCreator,
            IRolesProvider rolesProvider
        )
        {
            _environmentSettings = environmentSettings;
            _auth0ManagementApiClientCreator = auth0ManagementApiClientCreator;
            _rolesProvider = rolesProvider;
        }

        public async Task<string> CreateAsync(User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);

            global::Auth0.ManagementApi.Models.User user = null;
            try
            {
                user = await client.Users.GetAsync(string.Concat("auth0|", internalUser.Id.ToString("N")),
                    cancellationToken: cancellationToken);
                // we're likely in a retry for an exception that occured after the user was successfully created in auth0.
            }
            catch (ErrorApiException ex) when (ex.ApiError?.ErrorCode == "inexistent_user")
            {
            }

            if (user == null)
            {
                var userCreateRequest = new UserCreateRequest
                {
                    Connection = _environmentSettings.Auth0.Management.UserDatabaseIdentifier,
                    UserId = internalUser.Id.ToString("N"),
                    Email = internalUser.Email,
                    EmailVerified = false,
                    Password = string.Concat(internalUser.Id.ToString(), "@", new Random().Next(9999)),
                    FirstName = internalUser.GivenName,
                    LastName = $"{internalUser.MiddleName} {internalUser.FamilyName}".Trim(),
                    VerifyEmail = false
                };
                user = await client.Users.CreateAsync(userCreateRequest, cancellationToken);
            }

            var roles = _rolesProvider.Get()
                .Where(x => internalUser.UserRoles.Any(userRole => userRole.RoleId == x.Id))
                .Select(x => x.ExternalId)
                .ToArray();
            var assignRolesRequest = new AssignRolesRequest { Roles = roles };
            await client.Users.AssignRolesAsync(user.UserId, assignRolesRequest, cancellationToken);

            return user.UserId;
        }

        public async Task<string> GetChangePasswordUrlAsync(User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            var passwordChangeTicketRequest = new PasswordChangeTicketRequest
            {
                UserId = internalUser.ExternalId,
                MarkEmailAsVerified = true,
                ResultUrl = _environmentSettings.Auth0.RedirectUrl,
                IncludeEmailInRedirect = true,
                Ttl = 604800 // one week
            };
            return (await client.Tickets.CreatePasswordChangeTicketAsync(passwordChangeTicketRequest,
                cancellationToken)).Value;
        }

        public async Task SyncAsync(User internalUser, bool verifyEmail, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            var request = new UserUpdateRequest
            {
                Connection = _environmentSettings.Auth0.Management.UserDatabaseIdentifier,
                Email = internalUser.Email,
                FirstName = internalUser.GivenName,
                LastName = $"{internalUser.MiddleName} {internalUser.FamilyName}".Trim()
            };
            if (verifyEmail)
            {
                request.EmailVerified = false;
                request.VerifyEmail = true;
            }

            await client.Users.UpdateAsync(internalUser.ExternalId, request, cancellationToken);

            await SyncRolesAsync(internalUser, cancellationToken);
        }

        public async Task DeleteAsync(User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);
            await client.Users.DeleteAsync(internalUser.ExternalId);
        }

        private async Task SyncRolesAsync(User internalUser, CancellationToken cancellationToken = default)
        {
            var client = await _auth0ManagementApiClientCreator.GetClient(cancellationToken);

            var roleIdsAssignedToUser = _rolesProvider.Get()
                .Where(x => internalUser.UserRoles.Any(y => y.RoleId == x.Id))
                .Select(x => x.ExternalId)
                .ToList();

            var roleIdsAssignedToUserInAuth0 =
                (await client.Users.GetRolesAsync(internalUser.ExternalId, null, cancellationToken))
                .Select(x => x.Id);

            var rolesToAdd = roleIdsAssignedToUser
                .Where(internalRoleId => roleIdsAssignedToUserInAuth0.All(auth0RoleId => auth0RoleId != internalRoleId))
                .ToArray();
            if (rolesToAdd.Length > 0)
            {
                var assignRolesRequest = new AssignRolesRequest { Roles = rolesToAdd };
                await client.Users.AssignRolesAsync(internalUser.ExternalId, assignRolesRequest, cancellationToken);
            }

            var rolesToRemove = roleIdsAssignedToUserInAuth0
                .Where(auth0RoleId => roleIdsAssignedToUser.All(internalRoleId => internalRoleId != auth0RoleId))
                .ToArray();
            if (rolesToRemove.Length > 0)
            {
                var removeRolesRequest = new AssignRolesRequest { Roles = rolesToRemove };
                await client.Users.RemoveRolesAsync(internalUser.ExternalId, removeRolesRequest, cancellationToken);
            }
        }
    }
}