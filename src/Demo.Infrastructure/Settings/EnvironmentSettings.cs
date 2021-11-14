namespace Demo.Infrastructure.Settings
{
    public class EnvironmentSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public Auth0 Auth0 { get; set; }
    }
}
