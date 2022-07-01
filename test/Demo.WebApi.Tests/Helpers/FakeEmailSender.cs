using System.Threading;
using System.Threading.Tasks;
using Demo.Application.Shared.Interfaces;

namespace Demo.WebApi.Tests.Helpers
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendAsync(string toAddress, string subject, string htmlContent,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}