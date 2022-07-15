using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Role;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Infrastructure.Services
{
    internal class UserPermissionsProvider : IUserPermissionsProvider
    {
        private readonly IUserProvider _userProvider;
        private readonly IPermissionsProvider _permissionsProvider;

        public UserPermissionsProvider(
            IUserProvider userProvider,
            IPermissionsProvider permissionsProvider
        )
        {
            _userProvider = userProvider;
            _permissionsProvider = permissionsProvider;
        }

        public Task<List<Permission>> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            return GetAsync(userId, false, cancellationToken);
        }

        public async Task<List<Permission>> GetAsync(Guid userId, bool refreshCache, CancellationToken cancellationToken)
        {
            var user = await _userProvider.GetAsync(userId, cancellationToken);

            var userRoleIds = user.UserRoles.Select(x => x.RoleId);

            var allPermissions = await _permissionsProvider.GetAsync(cancellationToken);

            var userPermissions = allPermissions
                .Where(permission => permission.RolePermissions.Any(rolePermission => userRoleIds.Contains(rolePermission.RoleId)))
                .ToList();

            return userPermissions;
        }
    }
}