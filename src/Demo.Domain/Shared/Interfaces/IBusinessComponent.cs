using Demo.Domain.Shared.BusinessComponent;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IBusinessComponent<T> where T : IEntity
    {
        T Entity { get; }
        Guid EntityId { get; }
        IBusinessComponentState State { get; }
        IBusinessComponent<T> WithOptions(Action<IBusinessComponentOptions> action);
        Task NewAsync(CancellationToken cancellationToken);
        Task GetAsync(Guid id, CancellationToken cancellationToken);
        void With(Action<T> action);
        Task CreateAsync(CancellationToken cancellationToken);
        Task UpdateAsync(CancellationToken cancellationToken);
        Task UpsertAsync(CancellationToken cancellationToken);
        Task DeleteAsync(CancellationToken cancellationToken);
        void PublishDomainEventAfterCommit(IDomainEvent domainEvent);
    }
}
