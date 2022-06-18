namespace Demo.Domain.Shared.Interfaces
{
    public interface IDomainEntityOptions
    {
        bool IncludeDeleted { get; set; }
        bool AsNoTracking { get; set; }
        bool DisableSoftDelete { get; set; }
    }
}
