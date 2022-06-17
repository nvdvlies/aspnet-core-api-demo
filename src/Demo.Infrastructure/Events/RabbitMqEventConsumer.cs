using System;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Demo.Infrastructure.Events
{
    public class RabbitMqEventConsumer : IConsumer<RabbitMqEvent>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILogger<RabbitMqEventConsumer> _logger;
        private readonly IMediator _mediator;

        public RabbitMqEventConsumer(
            IMediator mediator,
            ILogger<RabbitMqEventConsumer> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task Consume(ConsumeContext<RabbitMqEvent> context)
        {
            _correlationIdProvider.SwitchToCorrelationId(context.CorrelationId ?? Guid.NewGuid());
            using (LogContext.PushProperty("CorrelationId", _correlationIdProvider.Id))
            {
                var @event = context.Message.ToEvent();
                _logger.LogInformation("Consuming {0} with type '{1}'", nameof(RabbitMqEvent),
                    context.Message.ContentType);
                await _mediator.Publish(@event, context.CancellationToken);
            }
        }
    }
}