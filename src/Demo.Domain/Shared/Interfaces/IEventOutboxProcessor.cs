using Demo.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IEventOutboxProcessor
    {
        Task AddToOutboxAsync(Event @event, CancellationToken cancellationToken);
        Task PublishAllAsync(CancellationToken cancellationToken);
    }
}
