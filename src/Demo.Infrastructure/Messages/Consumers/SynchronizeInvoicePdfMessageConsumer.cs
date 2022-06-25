using System;
using System.Threading.Tasks;
using Demo.Messages.Invoice;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class SynchronizeInvoicePdfMessageConsumer : BaseConsumer<SynchronizeInvoicePdfMessage>,
        IConsumer<SynchronizeInvoicePdfMessage>
    {
        public SynchronizeInvoicePdfMessageConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task ConsumeInternal(ConsumeContext<SynchronizeInvoicePdfMessage> context)
        {
            Logger.LogInformation($"Consuming {nameof(SynchronizeInvoicePdfMessage)}");
            var message = context.Message;
            await Mediator.Send(message, context.CancellationToken);
        }
    }
}
