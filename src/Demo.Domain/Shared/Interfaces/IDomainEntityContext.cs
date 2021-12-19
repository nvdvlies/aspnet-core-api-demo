using Demo.Domain.Shared.DomainEntity;

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
        void PublishDomainEventAfterCommit(IDomainEvent notification);
    }
}