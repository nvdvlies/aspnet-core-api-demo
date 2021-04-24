using Demo.Domain.Shared.BusinessComponent;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IBusinessComponentContext<T> where T : IEntity
    {
        EditMode EditMode { get; set; }
        bool IsNewEntity { get; }
        T Entity { get; set; }
        T Pristine { get; }
        IBusinessComponentState State { get; }
        PerformanceMeasurements PerformanceMeasurements { get; }
        void PublishDomainEventAfterCommit(IDomainEvent notification);
    }
}