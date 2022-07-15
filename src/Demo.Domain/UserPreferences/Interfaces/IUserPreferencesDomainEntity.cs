using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.UserPreferences.Interfaces
{
    public interface IUserPreferencesDomainEntity : IDomainEntity<UserPreferences>
    {
        Task GetAsync(CancellationToken cancellationToken = default);
    }
}
