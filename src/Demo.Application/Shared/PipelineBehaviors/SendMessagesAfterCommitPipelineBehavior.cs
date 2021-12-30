using Demo.Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class SendMessagesAfterCommitPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMessageOutboxProcessor _messageOutboxProcessor;
        private readonly ILogger<SendMessagesAfterCommitPipelineBehavior<TRequest, TResponse>> _logger;

        public SendMessagesAfterCommitPipelineBehavior(
            IMessageOutboxProcessor messageOutboxProcessor,
            ILogger<SendMessagesAfterCommitPipelineBehavior<TRequest, TResponse>> logger
        )
        {
            _messageOutboxProcessor = messageOutboxProcessor;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            await _messageOutboxProcessor.SendAllAsync(cancellationToken);

            return response;
        }
    }
}
