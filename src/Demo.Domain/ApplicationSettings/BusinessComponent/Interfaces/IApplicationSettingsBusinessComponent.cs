using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.BusinessComponent.Interfaces
{
    public interface IApplicationSettingsBusinessComponent : IBusinessComponent<ApplicationSettings>
    {
        Task GetAsync(CancellationToken cancellationToken = default);
    }
}
