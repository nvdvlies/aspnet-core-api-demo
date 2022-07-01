using System;
using System.Threading.Tasks;
using Demo.Messages.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class CreateAuth0UserMessageConsumer : BaseConsumer<CreateAuth0UserMessage>,
        IConsumer<CreateAuth0UserMessage>
    {
        public CreateAuth0UserMessageConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task ConsumeInternal(ConsumeContext<CreateAuth0UserMessage> context)
        {
            Logger.LogInformation($"Consuming {nameof(CreateAuth0UserMessage)}");
            var message = context.Message;
            await Mediator.Send(message, context.CancellationToken);
        }
    }
}