using Demo.Messages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces
{
    public interface IMessageSender
    {
        Task SendAsync(IMessage message, CancellationToken cancellationToken);
        Task SendAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken);
    }
}
