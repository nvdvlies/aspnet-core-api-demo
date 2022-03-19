using Demo.Application.Shared.Interfaces;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxEvent;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class ProcessOutboxEventCreatedEventsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IOutboxEventCreatedEvents _outboxEventCreatedEvents;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<ProcessOutboxEventCreatedEventsPipelineBehavior<TRequest, TResponse>> _logger;

        public ProcessOutboxEventCreatedEventsPipelineBehavior(
            IOutboxEventCreatedEvents outboxEventCreatedEvents,
            ICorrelationIdProvider correlationIdProvider,
            IEventPublisher eventPublisher,
            ILogger<ProcessOutboxEventCreatedEventsPipelineBehavior<TRequest, TResponse>> logger
        )
        {
            _outboxEventCreatedEvents = outboxEventCreatedEvents;
            _correlationIdProvider = correlationIdProvider;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            try
            {
                foreach (var outboxEventCreatedEvent in _outboxEventCreatedEvents)
                {
                    await _eventPublisher.PublishAsync(outboxEventCreatedEvent, cancellationToken);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish {nameof(OutboxEventCreatedEvent)} event(s). The affected event(s) will be processed later by the outbox monitoring service.");
            }

            return response;
        }
    }
}
