using Demo.Events;
using Demo.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    public interface IDomainEntity<T> where T : IEntity
    {
        T Entity { get; }
        Guid EntityId { get; }
        IDomainEntityState State { get; }
        IDomainEntity<T> WithOptions(Action<IDomainEntityOptions> action);
        Task NewAsync(CancellationToken cancellationToken);
        Task GetAsync(Guid id, CancellationToken cancellationToken);
        void With(Action<T> action);
        Task CreateAsync(CancellationToken cancellationToken);
        Task UpdateAsync(CancellationToken cancellationToken);
        Task UpsertAsync(CancellationToken cancellationToken);
        Task DeleteAsync(CancellationToken cancellationToken);
        Task PublishIntegrationEventAsync<E>(Event<E> @event, CancellationToken cancellationToken) where E : IEventData;
        Task SendMessageToQueueAsync<M>(Message<M> message, CancellationToken cancellationToken) where M : IMessageData;
    }
}
