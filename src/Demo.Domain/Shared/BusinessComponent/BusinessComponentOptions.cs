namespace Demo.Domain.Shared.BusinessComponent
{
    public class BusinessComponentOptions
    {
        public bool IncludeDeleted { get; set; } = false;
        public bool AsNoTracking { get; set; } = false;
        public bool DisableSoftDelete { get; set; } = false;
    }
}
