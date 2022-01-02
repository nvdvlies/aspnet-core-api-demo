using Demo.Domain.Shared.DomainEntity;
using Demo.Events;
using Demo.Messages;
using System.Threading;
using System.Threading.Tasks;

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
        Task PublishIntegrationEventAsync<E, D>(Event<E, D> @event, CancellationToken cancellationToken) where D : IEventData;
        Task SendMessageToQueueAsync<M>(Message<M> message, CancellationToken cancellationToken) where M : IMessageData;
    }
}