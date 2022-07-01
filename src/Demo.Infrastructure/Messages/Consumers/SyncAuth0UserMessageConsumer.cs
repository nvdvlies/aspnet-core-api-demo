using System;
using System.Threading.Tasks;
using Demo.Messages.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class SyncAuth0UserMessageConsumer : BaseConsumer<SyncAuth0UserMessage>, IConsumer<SyncAuth0UserMessage>
    {
        public SyncAuth0UserMessageConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task ConsumeInternal(ConsumeContext<SyncAuth0UserMessage> context)
        {
            Logger.LogInformation($"Consuming {nameof(SyncAuth0UserMessage)}");
            var message = context.Message;
            await Mediator.Send(message, context.CancellationToken);
        }
    }
}