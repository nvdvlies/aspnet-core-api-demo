using System.Threading;
using System.Threading.Tasks;
using Demo.Events;

namespace Demo.Application.Shared.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken);
    }
}
