using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Demo.Infrastructure.Settings;
using Demo.Messages;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Messages
{
    internal class ServiceBusQueueMessageSender : IMessageSender
    {
        private readonly ServiceBusClient _client;

        public ServiceBusQueueMessageSender(EnvironmentSettings environmentSettings)
        {
            _client = new ServiceBusClient(environmentSettings.ServiceBus.Namespace, new DefaultAzureCredential());
        }

        public async Task SendAsync(Message message, CancellationToken cancellationToken)
        {
            var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));
            var queueName = "todo"; // TODO
            var sender = _client.CreateSender(queueName);
            await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
        }
    }
}
