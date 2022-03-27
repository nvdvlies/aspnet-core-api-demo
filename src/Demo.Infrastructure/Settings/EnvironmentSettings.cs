namespace Demo.Infrastructure.Settings
{
    public class EnvironmentSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public Auth0 Auth0 { get; set; } = new Auth0();
        public EventGrid EventGrid { get; set; } = new EventGrid();
        public ServiceBus ServiceBus { get; set; } = new ServiceBus();
        public Redis Redis { get; set; } = new Redis();
    }
}
