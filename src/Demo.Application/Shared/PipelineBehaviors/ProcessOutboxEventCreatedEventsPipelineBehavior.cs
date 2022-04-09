using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxEvent;
using Demo.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class ProcessOutboxEventCreatedEventsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Lazy<IOutboxEventCreatedEvents> _outboxEventCreatedEvents;
        private readonly Lazy<IEventPublisher> _eventPublisher;
        private readonly ILogger<ProcessOutboxEventCreatedEventsPipelineBehavior<TRequest, TResponse>> _logger;

        public ProcessOutboxEventCreatedEventsPipelineBehavior(
            Lazy<IOutboxEventCreatedEvents> outboxEventCreatedEvents,
            Lazy<IEventPublisher> eventPublisher,
            ILogger<ProcessOutboxEventCreatedEventsPipelineBehavior<TRequest, TResponse>> logger
        )
        {
            _outboxEventCreatedEvents = outboxEventCreatedEvents;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (request is ICommand || request is IMessage)
            {
                try
                {
                    foreach (var outboxEventCreatedEvent in _outboxEventCreatedEvents.Value)
                    {
                        await _eventPublisher.Value.PublishAsync(outboxEventCreatedEvent, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to publish {nameof(OutboxEventCreatedEvent)} event(s). The affected event(s) will be processed later by the outbox monitoring service.");
                }
            }

            return response;
        }
    }
}
