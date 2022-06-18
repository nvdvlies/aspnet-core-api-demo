using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Application.ApplicationSettings.Commands.SaveApplicationSettings;

namespace Demo.WebApi.Tests.Controllers.ApplicationSettings.Helpers
{
    internal class ApplicationSettingsOperations
    {
        private readonly HttpClient _httpClient;

        public ApplicationSettingsOperations(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Get()
        {
            return await _httpClient.GetAsync("/api/applicationsettings");
        }

        public async Task<HttpResponseMessage> Save(SaveApplicationSettingsCommand command)
        {
            return await _httpClient.PutAsJsonAsync("/api/applicationsettings", command);
        }
    }
}
