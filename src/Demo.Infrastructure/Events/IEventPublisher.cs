using Demo.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Events
{
    internal interface IEventPublisher
    {
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken);
    }
}
