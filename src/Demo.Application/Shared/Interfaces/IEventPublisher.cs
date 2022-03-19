using Demo.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken);
    }
}
