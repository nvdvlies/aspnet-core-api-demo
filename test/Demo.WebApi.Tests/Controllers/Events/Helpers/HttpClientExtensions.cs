using System.Net.Http;

namespace Demo.WebApi.Tests.Controllers.Events.Helpers
{
    internal static class HttpClientExtensions
    {
        public static EventOperations EventsController(this HttpClient httpClient)
        {
            return new EventOperations(httpClient);
        }
    }
}
