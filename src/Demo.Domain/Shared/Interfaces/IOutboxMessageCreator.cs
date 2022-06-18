using System.Threading;
using System.Threading.Tasks;
using Demo.Messages;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IOutboxMessageCreator
    {
        Task CreateAsync(IMessage message, CancellationToken cancellationToken);
    }
}
