using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.FeatureFlagSettings.Interfaces
{
    public interface IFeatureFlagSettingsProvider
    {
        Task<FeatureFlagSettings> GetAsync(CancellationToken cancellationToken);
        Task<FeatureFlagSettings> GetAsync(bool refreshCache, CancellationToken cancellationToken);
    }
}
