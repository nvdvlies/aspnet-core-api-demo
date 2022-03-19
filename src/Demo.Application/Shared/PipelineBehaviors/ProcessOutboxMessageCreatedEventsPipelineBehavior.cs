using Demo.Application.Shared.Interfaces;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxMessage;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class ProcessOutboxMessageCreatedEventsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IOutboxMessageCreatedEvents _outboxMessageCreatedEvents;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<ProcessOutboxMessageCreatedEventsPipelineBehavior<TRequest, TResponse>> _logger;

        public ProcessOutboxMessageCreatedEventsPipelineBehavior(
            IOutboxMessageCreatedEvents outboxMessageCreatedEvents,
            ICorrelationIdProvider correlationIdProvider,
            IEventPublisher eventPublisher,
            ILogger<ProcessOutboxMessageCreatedEventsPipelineBehavior<TRequest, TResponse>> logger
        )
        {
            _outboxMessageCreatedEvents = outboxMessageCreatedEvents;
            _correlationIdProvider = correlationIdProvider;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            try
            {
                foreach (var outboxMessageCreatedEvent in _outboxMessageCreatedEvents)
                {
                    await _eventPublisher.PublishAsync(outboxMessageCreatedEvent, cancellationToken);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish {nameof(OutboxMessageCreatedEvent)} event(s). The affected message(s) will be processed later by the outbox monitoring service.");
            }

            return response;
        }
    }
}
