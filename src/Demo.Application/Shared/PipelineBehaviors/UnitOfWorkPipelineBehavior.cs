using Demo.Application.Shared.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkPipelineBehavior(
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            await _unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
    }
}
