using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Shared.DomainEntity;

internal class DomainEntityOptions : IDomainEntityOptions, IReadonlyDomainEntityOptions
{
    public bool IncludeDeleted { get; set; } = false;
    public bool AsNoTracking { get; set; } = false;
    public bool DisableSoftDelete { get; set; } = false;
}
