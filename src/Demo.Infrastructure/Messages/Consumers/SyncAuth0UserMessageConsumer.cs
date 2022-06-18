using System;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Messages.User;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class SyncAuth0UserMessageConsumer : IConsumer<SyncAuth0UserMessage>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILogger<SyncAuth0UserMessageConsumer> _logger;
        private readonly IMediator _mediator;

        public SyncAuth0UserMessageConsumer(
            IMediator mediator,
            ILogger<SyncAuth0UserMessageConsumer> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task Consume(ConsumeContext<SyncAuth0UserMessage> context)
        {
            _correlationIdProvider.SwitchToCorrelationId(context.CorrelationId ?? Guid.NewGuid());
            using (LogContext.PushProperty("CorrelationId", _correlationIdProvider.Id))
            {
                _logger.LogInformation($"Consuming {nameof(SyncAuth0UserMessage)}");
                var message = context.Message;
                await _mediator.Send(message, context.CancellationToken);
            }
        }
    }
}
