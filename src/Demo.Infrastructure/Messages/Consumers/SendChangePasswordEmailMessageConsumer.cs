using System;
using System.Threading.Tasks;
using Demo.Messages.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class SendChangePasswordEmailMessageConsumer : BaseMessageConsumer<SendChangePasswordEmailMessage>,
        IConsumer<SendChangePasswordEmailMessage>
    {
        public SendChangePasswordEmailMessageConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task ConsumeInternal(ConsumeContext<SendChangePasswordEmailMessage> context)
        {
            Logger.LogInformation($"Consuming {nameof(SendChangePasswordEmailMessage)}");
            var message = context.Message;
            return Mediator.Send(message, context.CancellationToken);
        }
    }
}
