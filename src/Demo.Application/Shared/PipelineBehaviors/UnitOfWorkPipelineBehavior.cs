using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;
using MediatR;

namespace Demo.Application.Shared.PipelineBehaviors
{
    public class UnitOfWorkPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;

        public UnitOfWorkPipelineBehavior(
            Lazy<IUnitOfWork> unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            await _unitOfWork.Value.CommitAsync(cancellationToken);

            return response;
        }
    }
}
