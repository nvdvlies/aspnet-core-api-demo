using Auth0.ManagementApi;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Auth0
{
    public interface IAuth0ManagementApiClientProvider
    {
        Task<ManagementApiClient> GetClient(CancellationToken cancellationToken = default);
    }
}
