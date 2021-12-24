using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class PublishDomainEventsAfterCommitPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMediator _mediator;
        private readonly IPublishDomainEventAfterCommitQueue _publishDomainEventAfterCommitQueue;
        private readonly ILogger<PublishDomainEventsAfterCommitPipelineBehavior<TRequest, TResponse>> _logger;

        public PublishDomainEventsAfterCommitPipelineBehavior(
            IMediator mediator,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            ILogger<PublishDomainEventsAfterCommitPipelineBehavior<TRequest, TResponse>> logger
        )
        {
            _mediator = mediator;
            _publishDomainEventAfterCommitQueue = publishDomainEventAfterCommitQueue;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            while (_publishDomainEventAfterCommitQueue.Count > 0)
            {
                var domainEvent = _publishDomainEventAfterCommitQueue.Dequeue();

                _logger.LogInformation($"Publishing {domainEvent.GetType().Name} domain event");

                await _mediator.Publish(domainEvent, cancellationToken);
            }

            return response;
        }
    }
}
