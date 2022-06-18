using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Demo.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Xunit;

namespace Demo.WebApi.Tests.Helpers
{
    public class SharedFixture : ICollectionFixture<CustomWebApplicationFactory>
    {
        public readonly Checkpoint Checkpoint = new()
        {
            SchemasToInclude = new[] { "demo" }, TablesToIgnore = new[] { "Role" }, WithReseed = true
        };

        public readonly HttpClient Client;
        public readonly CustomWebApplicationFactory Factory;
        public readonly HubConnection HubConnection;

        public SharedFixture()
        {
            Factory = new CustomWebApplicationFactory();
            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.DefaultScheme);

            HubConnection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl("http://localhost/hub",
                    o => { o.HttpMessageHandlerFactory = _ => Factory.Server.CreateHandler(); })
                .Build();
            HubConnection.StartAsync().Wait();

            MigrateDatabaseAsync().Wait();
        }

        private async Task MigrateDatabaseAsync()
        {
            using var scope = Factory.Services.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await applicationDbContext.Database.MigrateAsync();
        }
    }
}
