using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.UserPreferences.Interfaces;

public interface IUserPreferencesProvider
{
    Task<UserPreferences> GetAsync(CancellationToken cancellationToken);
    Task<UserPreferences> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    Task RemoveAsync(Guid userId, CancellationToken cancellationToken);
}
