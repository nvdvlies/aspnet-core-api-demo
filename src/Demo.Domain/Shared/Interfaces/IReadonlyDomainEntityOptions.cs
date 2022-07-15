namespace Demo.Domain.Shared.Interfaces
{
    public interface IReadonlyDomainEntityOptions
    {
        bool IncludeDeleted { get; }
        bool AsNoTracking { get; }
        bool DisableSoftDelete { get; }
    }
}
