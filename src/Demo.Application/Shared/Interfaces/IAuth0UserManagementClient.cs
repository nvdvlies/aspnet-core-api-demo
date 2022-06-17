using Demo.Domain.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces
{
    public interface IAuth0UserManagementClient
    {
        Task<string> CreateAsync(User internalUser, CancellationToken cancellationToken = default);
        Task<string> GetChangePasswordUrl(User internalUser, CancellationToken cancellationToken = default);
        Task SyncAsync(User internalUser, bool verifyEmail, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
