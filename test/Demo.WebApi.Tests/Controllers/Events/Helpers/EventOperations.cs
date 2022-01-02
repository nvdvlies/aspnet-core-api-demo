using Azure.Messaging.EventGrid;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Controllers.Events.Helpers
{
    internal class EventOperations
    {
        private readonly HttpClient _httpClient;

        public EventOperations(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> PostAsync(EventGridEvent[] eventGridEvents)
        {
            return await _httpClient.PostAsJsonAsync("/api/events", eventGridEvents);
        }
    }
}
