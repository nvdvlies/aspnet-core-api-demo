using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.Interfaces
{
    public interface IApplicationSettingsProvider
    {
        Task<ApplicationSettings> GetAsync(CancellationToken cancellationToken);
        Task<ApplicationSettings> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    }
}