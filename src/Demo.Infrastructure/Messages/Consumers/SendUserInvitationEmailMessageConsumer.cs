using System;
using System.Threading.Tasks;
using Demo.Messages.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers;

public class SendUserInvitationEmailMessageConsumer : BaseMessageConsumer<SendUserInvitationEmailMessage>,
    IConsumer<SendUserInvitationEmailMessage>
{
    public SendUserInvitationEmailMessageConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task ConsumeInternal(ConsumeContext<SendUserInvitationEmailMessage> context)
    {
        Logger.LogInformation($"Consuming {nameof(SendUserInvitationEmailMessage)}");
        var message = context.Message;
        return Mediator.Send(message, context.CancellationToken);
    }
}
