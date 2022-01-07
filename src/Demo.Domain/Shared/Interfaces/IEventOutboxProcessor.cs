using Demo.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IEventOutboxProcessor
    {
        Task AddToOutboxAsync(IEvent @event, CancellationToken cancellationToken);
        Task PublishAllAsync(CancellationToken cancellationToken);
    }
}
