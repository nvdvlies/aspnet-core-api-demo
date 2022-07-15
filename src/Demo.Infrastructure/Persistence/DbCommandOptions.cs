using Demo.Domain.Shared.Interfaces;

namespace Demo.Infrastructure.Persistence
{
    public class DbCommandOptions : IDbCommandOptions
    {
        public bool AsNoTracking { get; set; } = false;
    }
}
