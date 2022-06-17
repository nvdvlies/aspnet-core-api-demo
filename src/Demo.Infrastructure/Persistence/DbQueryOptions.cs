using Demo.Domain.Shared.Interfaces;

namespace Demo.Infrastructure.Persistence
{
    public class DbQueryOptions : IDbQueryOptions
    {
        public bool IncludeDeleted { get; set; } = false;
    }
}