using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.User;

namespace Demo.Application.Shared.Interfaces;

public interface IAuth0UserManagementClient
{
    Task<string> CreateAsync(User internalUser, CancellationToken cancellationToken = default);
    Task<string> GetChangePasswordUrlAsync(User internalUser, CancellationToken cancellationToken = default);
    Task SyncAsync(User internalUser, bool verifyEmail, CancellationToken cancellationToken = default);
    Task DeleteAsync(User internalUser, CancellationToken cancellationToken = default);
}
