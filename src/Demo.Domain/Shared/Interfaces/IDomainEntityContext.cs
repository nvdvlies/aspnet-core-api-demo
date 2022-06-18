using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Events;
using Demo.Messages;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IDomainEntityContext<T> where T : IEntity
    {
        EditMode EditMode { get; set; }
        bool IsNewEntity { get; }
        T Entity { get; set; }
        T Pristine { get; }
        IDomainEntityState State { get; }
        PerformanceMeasurements PerformanceMeasurements { get; }
        Task AddEventAsync(IEvent @event, CancellationToken cancellationToken);
        Task AddMessageAsync(IMessage message, CancellationToken cancellationToken);
    }
}
