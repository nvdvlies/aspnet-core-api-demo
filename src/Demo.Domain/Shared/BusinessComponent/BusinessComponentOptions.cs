using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.Shared.BusinessComponent
{
    internal class BusinessComponentOptions : IBusinessComponentOptions, IReadonlyBusinessComponentOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool AsNoTracking { get; set; } = false;
        public bool DisableSoftDelete { get; set; } = false;
    }
}
