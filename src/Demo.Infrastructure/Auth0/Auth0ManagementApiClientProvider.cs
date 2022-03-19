using Auth0.ManagementApi;
using Demo.Infrastructure.Settings;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Auth0
{
    internal class Auth0ManagementApiClientProvider : IAuth0ManagementApiClientProvider
    {
        private readonly EnvironmentSettings _environmentSettings;
        private readonly IManagementConnection _managementConnection;

        private ManagementApiClient _managementApiClient;

        public Auth0ManagementApiClientProvider(
            EnvironmentSettings environmentSettings,
            IManagementConnection managementConnection
        )
        {
            _environmentSettings = environmentSettings;
            _managementConnection = managementConnection;
        }

        public async Task<ManagementApiClient> GetClient(CancellationToken cancellationToken = default)
        {
            if (_managementApiClient != null)
            {
                return _managementApiClient;
            }
            var accessToken = await GetAccessToken(cancellationToken);
            _managementApiClient = new ManagementApiClient(
                accessToken,
                _environmentSettings.Auth0.Domain,
                _managementConnection);
            return _managementApiClient;
        }

        private async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
        {
            using var client = new HttpClient();
            client.BaseAddress = new System.Uri(_environmentSettings.Auth0.Domain);
            var response = await client.PostAsync("oauth/token", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                            { "grant_type", "client_credentials" },
                            { "client_id", _environmentSettings.Auth0.ClientId },
                            { "client_secret", _environmentSettings.Auth0.ClientSecret },
                            { "audience", $"{_environmentSettings.Auth0.Domain}api/v2/" }
                }
            ));
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var json = JObject.Parse(content);
            return json["access_token"].Value<string>();
        }
    }
}
