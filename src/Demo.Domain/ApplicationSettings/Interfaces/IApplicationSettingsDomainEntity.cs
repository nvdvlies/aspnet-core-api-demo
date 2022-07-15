using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.ApplicationSettings.Interfaces
{
    public interface IApplicationSettingsDomainEntity : IDomainEntity<ApplicationSettings>
    {
        Task GetAsync(CancellationToken cancellationToken = default);
    }
}
