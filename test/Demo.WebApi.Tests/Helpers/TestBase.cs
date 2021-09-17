using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Settings;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Demo.WebApi.Tests.Helpers
{
    public abstract class TestBase : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly SharedFixture _fixture;

        protected readonly CustomWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly HubConnection _hubConnection;

        public TestBase(SharedFixture fixture)
        {
            _fixture = fixture;

            _factory = _fixture.Factory;
            _client = _fixture.Client;
            _hubConnection = _fixture.HubConnection;

            ResetDatabaseAsync().Wait();
        }

        public async Task ResetDatabaseAsync()
        {
            using var scope = _fixture.Factory.Services.CreateScope();
            var environmentSettings = scope.ServiceProvider.GetRequiredService<EnvironmentSettings>();
            await _fixture.Checkpoint.Reset(environmentSettings.ConnectionStrings.SqlDatabase);
        }

        public async Task AddAsExistingEntityAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _fixture.Factory.Services.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.Set<TEntity>().Add(entity);
            await applicationDbContext.SaveChangesAsync();
        }

        protected void WithRetry(Action act, TimeSpan timeout, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            act.Should().NotThrowAfter(timeout, pollInterval, because, becauseArgs);
        }
    }
}
