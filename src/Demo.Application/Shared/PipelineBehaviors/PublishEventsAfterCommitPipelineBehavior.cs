using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class PublishEventsAfterCommitPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEventOutboxProcessor _eventOutboxProcessor;
        private readonly ILogger<PublishEventsAfterCommitPipelineBehavior<TRequest, TResponse>> _logger;

        public PublishEventsAfterCommitPipelineBehavior(
            IEventOutboxProcessor eventOutboxProcessor,
            ILogger<PublishEventsAfterCommitPipelineBehavior<TRequest, TResponse>> logger
        )
        {
            _eventOutboxProcessor = eventOutboxProcessor;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            await _eventOutboxProcessor.PublishAllAsync(cancellationToken);

            return response;
        }
    }
}
