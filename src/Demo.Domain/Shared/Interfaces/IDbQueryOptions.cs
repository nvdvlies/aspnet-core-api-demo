namespace Demo.Domain.Shared.Interfaces;

public interface IDbQueryOptions
{
    bool IncludeDeleted { get; set; }
}
