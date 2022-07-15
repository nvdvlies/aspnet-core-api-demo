using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Infrastructure.Services
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly IUserPermissionsProvider _userPermissionsProvider;

        public PermissionChecker(
            ICurrentUserIdProvider currentUserIdProvider,
            IUserPermissionsProvider userPermissionsProvider
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _userPermissionsProvider = userPermissionsProvider;
        }

        public async Task<bool> HasPermissionAsync(string permissionName, CancellationToken cancellationToken = default)
        {
            var userPermissions = await _userPermissionsProvider.GetAsync(_currentUserIdProvider.Id, cancellationToken);

            return userPermissions.Any(permission => permission.Name == permissionName);
        }
    }
}