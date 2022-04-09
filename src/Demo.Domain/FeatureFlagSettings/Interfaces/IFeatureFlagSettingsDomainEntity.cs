using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.FeatureFlagSettings.Interfaces
{
    public interface IFeatureFlagSettingsDomainEntity : IDomainEntity<FeatureFlagSettings>
    {
        Task GetAsync(CancellationToken cancellationToken = default);
    }
}