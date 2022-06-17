using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.OutboxMessage;
using Demo.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class
        ProcessOutboxMessageCreatedEventsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Lazy<IEventPublisher> _eventPublisher;
        private readonly ILogger<ProcessOutboxMessageCreatedEventsPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly Lazy<IOutboxMessageCreatedEvents> _outboxMessageCreatedEvents;

        public ProcessOutboxMessageCreatedEventsPipelineBehavior(
            Lazy<IOutboxMessageCreatedEvents> outboxMessageCreatedEvents,
            Lazy<IEventPublisher> eventPublisher,
            ILogger<ProcessOutboxMessageCreatedEventsPipelineBehavior<TRequest, TResponse>> logger
        )
        {
            _outboxMessageCreatedEvents = outboxMessageCreatedEvents;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (request is ICommand || request is IMessage)
            {
                try
                {
                    foreach (var outboxMessageCreatedEvent in _outboxMessageCreatedEvents.Value)
                    {
                        await _eventPublisher.Value.PublishAsync(outboxMessageCreatedEvent, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        $"Failed to publish {nameof(OutboxMessageCreatedEvent)} event(s). The affected message(s) will be processed later by the outbox monitoring service.");
                }
            }

            return response;
        }
    }
}