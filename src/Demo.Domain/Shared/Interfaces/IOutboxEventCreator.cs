using System.Threading;
using System.Threading.Tasks;
using Demo.Events;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IOutboxEventCreator
    {
        Task CreateAsync(IEvent @event, CancellationToken cancellationToken);
    }
}
