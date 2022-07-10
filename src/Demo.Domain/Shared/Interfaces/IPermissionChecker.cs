using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IPermissionChecker
    {
        Task<bool> HasPermissionAsync(string permissionName, CancellationToken cancellationToken = default);
    }
}