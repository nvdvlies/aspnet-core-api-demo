using Demo.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Messages
{
    internal interface IMessageSender
    {
        Task SendAsync(Message message, CancellationToken cancellationToken);
    }
}
