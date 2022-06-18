using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.FeatureFlagSettings.Interfaces
{
    public interface IFeatureFlagSettingsDomainEntity : IDomainEntity<FeatureFlagSettings>
    {
        Task GetAsync(CancellationToken cancellationToken = default);
    }
}
