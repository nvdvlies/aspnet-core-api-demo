using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces
{
    public interface IOutboxEventPublisher
    {
        Task PublishAsync(Guid id, CancellationToken cancellationToken = default);
    }
}