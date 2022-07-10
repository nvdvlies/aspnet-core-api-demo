using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Infrastructure.Services
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IRolesProvider _rolesProvider;
        private readonly IUserProvider _userProvider;

        public PermissionChecker(
            ICurrentUserIdProvider currentUserIdProvider,
            IUserProvider userProvider,
            IRolesProvider rolesProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _userProvider = userProvider;
            _rolesProvider = rolesProvider;
        }

        public async Task<bool> HasPermissionAsync(string permissionName, CancellationToken cancellationToken = default)
        {
            var user = await _userProvider.GetAsync(_currentUserIdProvider.Id, cancellationToken);
            var roles = await _rolesProvider.GetAsync();

            var userRoleIds = user.UserRoles.Select(x => x.RoleId);
            return roles.Any(role =>
                userRoleIds.Contains(role.Id)
                && role.Permissions.Any(rolePermission => rolePermission.Permission.Name == permissionName)
            );
        }
    }
}