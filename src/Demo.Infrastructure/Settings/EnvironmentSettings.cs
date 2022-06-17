namespace Demo.Infrastructure.Settings
{
    public class EnvironmentSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public Auth0 Auth0 { get; set; } = new Auth0();
        public RabbitMq RabbitMq { get; set; } = new RabbitMq();
        public Redis Redis { get; set; } = new Redis();
        public ElasticSearch ElasticSearch { get; set; } = new ElasticSearch();
    }
}
