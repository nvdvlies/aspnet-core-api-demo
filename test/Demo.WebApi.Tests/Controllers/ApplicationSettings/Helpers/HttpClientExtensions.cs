using System.Net.Http;

namespace Demo.WebApi.Tests.Controllers.ApplicationSettings.Helpers
{
    internal static class HttpClientExtensions
    {
        public static ApplicationSettingsOperations ApplicationSettingsController(this HttpClient httpClient)
        {
            return new ApplicationSettingsOperations(httpClient);
        }
    }
}
