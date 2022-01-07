using Demo.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Messages
{
    internal interface IMessageSender
    {
        Task SendAsync(IMessage message, CancellationToken cancellationToken);
    }
}
