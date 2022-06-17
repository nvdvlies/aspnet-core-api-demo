using System;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Messages.Invoice;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class SynchronizeInvoicePdfMessageConsumer: IConsumer<SynchronizeInvoicePdfMessage>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SynchronizeInvoicePdfMessageConsumer> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public SynchronizeInvoicePdfMessageConsumer(
            IMediator mediator,
            ILogger<SynchronizeInvoicePdfMessageConsumer> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task Consume(ConsumeContext<SynchronizeInvoicePdfMessage> context)
        {
            _correlationIdProvider.SwitchToCorrelationId(context.CorrelationId ?? Guid.NewGuid());
            using (Serilog.Context.LogContext.PushProperty("CorrelationId", _correlationIdProvider.Id))
            {
                _logger.LogInformation($"Consuming {nameof(SynchronizeInvoicePdfMessage)}");
                var message = context.Message;
                await _mediator.Send(message, context.CancellationToken);
            }
        }
    }
}