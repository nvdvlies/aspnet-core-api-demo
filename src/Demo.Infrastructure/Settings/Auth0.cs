namespace Demo.Infrastructure.Settings;

public class Auth0
{
    public string Domain { get; set; }
    public string Audience { get; set; }
    public string RedirectUrl { get; set; }
    public Auth0Management Management { get; set; } = new();
    public IntegrationTestUser IntegrationTestUser { get; set; } = new();
}
