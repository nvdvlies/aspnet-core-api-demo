using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Settings;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        protected async Task ResetDatabaseAsync()
        {
            using var scope = _fixture.Factory.Services.CreateScope();
            var environmentSettings = scope.ServiceProvider.GetRequiredService<EnvironmentSettings>();
            await _fixture.Checkpoint.Reset(environmentSettings.ConnectionStrings.SqlDatabase);
        }

        protected async Task AddAsExistingEntityAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _fixture.Factory.Services.CreateScope();
            using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.Set<TEntity>().Add(entity);
            await applicationDbContext.SaveChangesAsync();
        }

        protected async Task AddAsExistingEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            using var scope = _fixture.Factory.Services.CreateScope();
            using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.Set<TEntity>().AddRange(entities);
            await applicationDbContext.SaveChangesAsync();
        }

        protected async Task<TEntity> FindExistingEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return (await FindExistingEntitiesAsync(predicate)).SingleOrDefault();
        }

        protected async Task<IEnumerable<TEntity>> FindExistingEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            using var scope = _fixture.Factory.Services.CreateScope();
            using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await applicationDbContext
                .Set<TEntity>()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        protected void WithRetry(Action assertion, TimeSpan timeout, TimeSpan pollInterval, string because = "", params object[] becauseArgs)
        {
            assertion.Should().NotThrowAfter(timeout, pollInterval, because, becauseArgs);
        }
    }
}
