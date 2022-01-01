using Demo.Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Demo.WebApi.Tests.Helpers
{
    public class SharedFixture : ICollectionFixture<CustomWebApplicationFactory>
    {
        public readonly CustomWebApplicationFactory Factory;
        public readonly HttpClient Client;
        public readonly HubConnection HubConnection;
        public readonly Checkpoint Checkpoint = new Checkpoint
        {
            SchemasToInclude = new[] {
                "demo"
            },
            WithReseed = true
        };

        public SharedFixture()
        {
            Factory = new CustomWebApplicationFactory();
            Client = Factory.CreateClient();

            HubConnection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl($"http://localhost/hub", o =>
                {
                    o.HttpMessageHandlerFactory = _ => Factory.Server.CreateHandler();
                })
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
