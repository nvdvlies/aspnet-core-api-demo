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
    public class DeleteAuth0UserMessageConsumer : IConsumer<DeleteAuth0UserMessage>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILogger<DeleteAuth0UserMessageConsumer> _logger;
        private readonly IMediator _mediator;

        public DeleteAuth0UserMessageConsumer(
            IMediator mediator,
            ILogger<DeleteAuth0UserMessageConsumer> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task Consume(ConsumeContext<DeleteAuth0UserMessage> context)
        {
            _correlationIdProvider.SwitchToCorrelationId(context.CorrelationId ?? Guid.NewGuid());
            using (LogContext.PushProperty("CorrelationId", _correlationIdProvider.Id))
            {
                _logger.LogInformation($"Consuming {nameof(DeleteAuth0UserMessage)}");
                var message = context.Message;
                await _mediator.Send(message, context.CancellationToken);
            }
        }
    }
}