using Demo.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IOutboxEventCreator
    {
        Task CreateAsync(IEvent @event, CancellationToken cancellationToken);
    }
}
