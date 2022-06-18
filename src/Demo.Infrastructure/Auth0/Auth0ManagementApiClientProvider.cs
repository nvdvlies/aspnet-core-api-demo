using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Demo.Infrastructure.Settings;
using Newtonsoft.Json.Linq;

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
                new Uri(_environmentSettings.Auth0.Domain).Host,
                _managementConnection);
            return _managementApiClient;
        }

        private async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_environmentSettings.Auth0.Domain);
            var response = await client.PostAsync("oauth/token", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", _environmentSettings.Auth0.Management.ClientId },
                    { "client_secret", _environmentSettings.Auth0.Management.ClientSecret },
                    { "audience", $"{_environmentSettings.Auth0.Domain}api/v2/" }
                }
            ), cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var json = JObject.Parse(content);
                return json["access_token"].Value<string>();
            }

            throw new Exception(
                $"Failed to retrieve access token for Auth0 Management Api. StatusCode: {response.StatusCode}. Content: {content}");
        }
    }
}
