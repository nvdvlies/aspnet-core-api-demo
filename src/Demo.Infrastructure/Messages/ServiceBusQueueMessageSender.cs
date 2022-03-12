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
#if !DEBUG
            _client = new ServiceBusClient(environmentSettings.ServiceBus.Namespace, new DefaultAzureCredential());
#endif
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));
#if DEBUG
            await Task.CompletedTask;
#else
            var sender = _client.CreateSender(message.Queue.ToString());
            await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
#endif
        }
    }
}
