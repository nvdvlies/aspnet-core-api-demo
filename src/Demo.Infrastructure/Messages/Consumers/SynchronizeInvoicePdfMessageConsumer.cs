using System;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Messages.Invoice;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class SynchronizeInvoicePdfMessageConsumer : IConsumer<SynchronizeInvoicePdfMessage>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILogger<SynchronizeInvoicePdfMessageConsumer> _logger;
        private readonly IMediator _mediator;

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
            using (LogContext.PushProperty("CorrelationId", _correlationIdProvider.Id))
            {
                _logger.LogInformation($"Consuming {nameof(SynchronizeInvoicePdfMessage)}");
                var message = context.Message;
                await _mediator.Send(message, context.CancellationToken);
            }
        }
    }
}
