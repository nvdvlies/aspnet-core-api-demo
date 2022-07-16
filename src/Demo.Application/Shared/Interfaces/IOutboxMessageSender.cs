using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Shared.Interfaces;

public interface IOutboxMessageSender
{
    Task SendAsync(Guid id, CancellationToken cancellationToken = default);
}
