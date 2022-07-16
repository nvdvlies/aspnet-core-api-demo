using System;
using System.Threading.Tasks;
using Demo.Messages.Invoice;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers;

public class SynchronizeInvoicePdfMessageConsumer : BaseMessageConsumer<SynchronizeInvoicePdfMessage>,
    IConsumer<SynchronizeInvoicePdfMessage>
{
    public SynchronizeInvoicePdfMessageConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task ConsumeInternal(ConsumeContext<SynchronizeInvoicePdfMessage> context)
    {
        Logger.LogInformation($"Consuming {nameof(SynchronizeInvoicePdfMessage)}");
        var message = context.Message;
        return Mediator.Send(message, context.CancellationToken);
    }
}
