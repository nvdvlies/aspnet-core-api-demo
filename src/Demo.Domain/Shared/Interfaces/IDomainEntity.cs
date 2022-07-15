using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Events;
using Demo.Messages;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IDomainEntity
    {
    }

    public interface IDomainEntity<T> : IDomainEntity where T : IEntity
    {
        T Entity { get; }
        Guid EntityId { get; }
        IDomainEntityState State { get; }
        IDomainEntity<T> WithOptions(Action<IDomainEntityOptions> action);
        Task NewAsync(CancellationToken cancellationToken = default);
        Task GetAsync(Guid id, CancellationToken cancellationToken = default);
        void With(Action<T> action);
        Task CreateAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task UpsertAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(CancellationToken cancellationToken = default);
        Task AddEventAsync(IEvent @event, CancellationToken cancellationToken = default);
        Task AddMessageAsync(IMessage message, CancellationToken cancellationToken = default);
    }
}