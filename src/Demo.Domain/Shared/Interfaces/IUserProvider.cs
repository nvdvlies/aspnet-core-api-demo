using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces;

public interface IUserProvider
{
    Task<User.User> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User.User> GetAsync(Guid id, bool refreshCache, CancellationToken cancellationToken = default);
}
