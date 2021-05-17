namespace Demo.Domain.Shared.Interfaces
{
    public interface IReadonlyBusinessComponentOptions
    {
        bool IncludeDeleted { get; }
        bool AsNoTracking { get; }
        bool DisableSoftDelete { get; }
    }
}
