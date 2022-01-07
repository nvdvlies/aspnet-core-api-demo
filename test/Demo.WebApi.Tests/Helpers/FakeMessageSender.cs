using Demo.Infrastructure.Messages;
using Demo.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Helpers
{
    public class FakeMessageSender : IMessageSender
    {
        public Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
