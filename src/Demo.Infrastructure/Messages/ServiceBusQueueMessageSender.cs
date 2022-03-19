using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Demo.Application.Shared.Interfaces;
using Demo.Infrastructure.Settings;
using Demo.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Messages
{
    internal class ServiceBusQueueMessageSender : IMessageSender
    {
        private readonly ILogger<ServiceBusQueueMessageSender> _logger;
        private readonly ServiceBusClient _client;

        public ServiceBusQueueMessageSender(
            ILogger<ServiceBusQueueMessageSender> logger,
            EnvironmentSettings environmentSettings
        )
        {
            _logger = logger;
            _client = new ServiceBusClient(environmentSettings.ServiceBus.Namespace, new DefaultAzureCredential());
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            var serviceBusMessage = message.ToServiceBusMessage();
            var sender = _client.CreateSender(message.Queue.ToString());
            await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
        }

        public async Task SendAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken)
        {
            if (messages.GroupBy(x => x.Queue).Count() > 1)
            {
                _logger.LogInformation("SendAsync() called with messages for multiple queues: {queues}", string.Join(",", messages.GroupBy(x => x.Queue).Select(x => x.Key)));
                throw new Exception("Sending a batch of messages is only supported for a single queue.");
            }

            var serviceBusMessages = messages.Select(message => message.ToServiceBusMessage());
            var sender = _client.CreateSender(messages.First().Queue.ToString());
            await sender.SendMessagesAsync(serviceBusMessages, cancellationToken);
        }
    }
}
