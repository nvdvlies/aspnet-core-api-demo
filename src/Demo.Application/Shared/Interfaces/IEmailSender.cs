using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces;

public interface IEmailSender
{
    Task SendAsync(string toAddress, string subject, string htmlContent,
        CancellationToken cancellationToken = default);
}
