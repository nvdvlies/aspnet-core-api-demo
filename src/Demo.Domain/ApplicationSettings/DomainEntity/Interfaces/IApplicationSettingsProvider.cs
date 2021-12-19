using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.DomainEntity.Interfaces
{
    public interface IApplicationSettingsProvider
    {
        Task<ApplicationSettings> GetAsync(CancellationToken cancellationToken);
        Task<ApplicationSettings> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    }
}
