using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.FeatureFlagSettings.Interfaces
{
    public interface IFeatureFlagChecker
    {
        Task<bool> IsEnabledAsync(string name, CancellationToken cancellationToken = default);
    }
}