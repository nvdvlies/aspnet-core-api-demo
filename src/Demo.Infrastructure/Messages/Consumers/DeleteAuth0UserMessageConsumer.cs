using System;
using System.Threading.Tasks;
using Demo.Messages.User;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class DeleteAuth0UserMessageConsumer : BaseConsumer<DeleteAuth0UserMessage>,
        IConsumer<DeleteAuth0UserMessage>
    {
        public DeleteAuth0UserMessageConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task ConsumeInternal(ConsumeContext<DeleteAuth0UserMessage> context)
        {
            Logger.LogInformation($"Consuming {nameof(DeleteAuth0UserMessage)}");
            var message = context.Message;
            await Mediator.Send(message, context.CancellationToken);
        }
    }
}
