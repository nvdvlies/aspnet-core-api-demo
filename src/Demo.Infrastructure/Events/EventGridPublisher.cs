using Azure.Identity;
using Azure.Messaging.EventGrid;
using Demo.Application.Shared.Interfaces;
using Demo.Events;
using Demo.Infrastructure.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Events
{
    internal class EventGridPublisher : IEventPublisher
    {
        private readonly EventGridPublisherClient _client;

        public EventGridPublisher(EnvironmentSettings environmentSettings)
        {
            _client = new EventGridPublisherClient(new Uri(environmentSettings.EventGrid.Endpoint), new DefaultAzureCredential());
        }

        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken)
        {
            var eventGridEvent = @event.ToEventGridEvent();
            var response = await _client.SendEventAsync(eventGridEvent, cancellationToken);

            if (!IsSuccessStatusCode(response.Status))
            {
                throw new Exception($"SendEventAsync failed. Status: {response.Status}, Reason: {response.ReasonPhrase}");
            }
        }

        private static bool IsSuccessStatusCode(int statusCode)
        {
            return (statusCode >= 200) && (statusCode <= 299);
        }
    }
}
