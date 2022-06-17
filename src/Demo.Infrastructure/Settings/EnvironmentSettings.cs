namespace Demo.Infrastructure.Settings
{
    public class EnvironmentSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new();
        public Auth0 Auth0 { get; set; } = new();
        public RabbitMq RabbitMq { get; set; } = new();
        public Redis Redis { get; set; } = new();
        public ElasticSearch ElasticSearch { get; set; } = new();
    }
}