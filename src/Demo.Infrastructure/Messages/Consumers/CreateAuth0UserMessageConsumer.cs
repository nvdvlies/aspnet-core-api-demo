using System;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Infrastructure.Events;
using Demo.Messages.User;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Infrastructure.Messages.Consumers
{
    public class CreateAuth0UserMessageConsumer: IConsumer<CreateAuth0UserMessage>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateAuth0UserMessageConsumer> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public CreateAuth0UserMessageConsumer(
            IMediator mediator,
            ILogger<CreateAuth0UserMessageConsumer> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task Consume(ConsumeContext<CreateAuth0UserMessage> context)
        {
            _correlationIdProvider.SwitchToCorrelationId(context.CorrelationId ?? Guid.NewGuid());
            using (Serilog.Context.LogContext.PushProperty("CorrelationId", _correlationIdProvider.Id))
            {
                _logger.LogInformation($"Consuming {nameof(CreateAuth0UserMessage)}");
                var message = context.Message;
                await _mediator.Send(message, context.CancellationToken);
            }
        }
    }
}