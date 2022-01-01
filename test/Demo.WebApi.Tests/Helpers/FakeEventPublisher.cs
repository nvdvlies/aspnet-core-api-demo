using Demo.Events;
using Demo.Infrastructure.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.WebApi.Tests.Helpers
{
    public class FakeEventPublisher : IEventPublisher
    {
        public Task PublishAsync(Event @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
