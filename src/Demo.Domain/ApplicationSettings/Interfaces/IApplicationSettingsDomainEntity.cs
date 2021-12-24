using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.Interfaces
{
    public interface IApplicationSettingsDomainEntity : IDomainEntity<ApplicationSettings>
    {
        Task GetAsync(CancellationToken cancellationToken = default);
    }
}
