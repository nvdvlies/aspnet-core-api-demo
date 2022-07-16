using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces;

public interface IAuditlogger<T> where T : IEntity
{
    Task CreateAuditLogAsync(T current, T previous, CancellationToken cancellationToken);
}
