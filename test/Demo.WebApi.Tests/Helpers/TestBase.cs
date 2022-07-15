﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture;
using Demo.Domain.Role;
using Demo.Domain.User;
using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Services;
using Demo.Infrastructure.Settings;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Xunit;

namespace Demo.WebApi.Tests.Helpers
{
    public abstract class TestBase : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IList<Permission> _allPermissions;
        private readonly SharedFixture _fixture;
        protected readonly Fixture AutoFixture;
        protected readonly HttpClient Client;

        protected readonly CustomWebApplicationFactory Factory;
        protected readonly HubConnection HubConnection;

        protected IServiceProvider ServiceProvider;

        public TestBase(SharedFixture fixture)
        {
            _fixture = fixture;

            Factory = _fixture.Factory;
            ServiceProvider = _fixture.Factory.Services;
            Client = _fixture.Client;
            HubConnection = _fixture.HubConnection;
            AutoFixture = AutoFixtureFactory.CreateAutofixtureWithDefaultConfiguration();

            ResetDatabaseAsync().Wait();

            var cache = ServiceProvider.GetRequiredService<IDistributedCache>();
            cache.Remove(RolesProvider.CacheKey);
            cache.Remove(FeatureFlagSettingsProvider.CacheKey);
            cache.Remove(ApplicationSettingsProvider.CacheKey);
            cache.Remove(PermissionsProvider.CacheKey);

            _allPermissions = FindExistingEntitiesAsync<Permission>(x => true).Result.ToList();
        }

        protected HttpClient CreateHttpClientWithCustomConfiguration(Action<IServiceCollection> servicesConfiguration)
        {
            var webhost = Factory
                .WithWebHostBuilder(builder => { builder.ConfigureTestServices(servicesConfiguration); });
            var client = webhost.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.DefaultScheme);
            ServiceProvider = webhost.Services;
            return client;
        }

        protected async Task ResetDatabaseAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var environmentSettings = scope.ServiceProvider.GetRequiredService<EnvironmentSettings>();

            await using var connection = new NpgsqlConnection(environmentSettings.ConnectionStrings.PostgresDatabase);
            await connection.OpenAsync();
            await _fixture.Checkpoint.Reset(connection);
        }

        protected async Task SetTestUserToUnauthenticated()
        {
            await SetTestUser(false, Array.Empty<string>());
        }

        protected async Task SetTestUserWithoutPermission()
        {
            await SetTestUser(true, Array.Empty<string>());
        }

        protected async Task SetTestUserWithPermission(string permissionName)
        {
            await SetTestUser(true, new[] { permissionName });
        }

        protected async Task SetTestUserWithPermission(IEnumerable<string> permissionNames)
        {
            await SetTestUser(true, permissionNames );
        }

        private async Task SetTestUser(bool isAuthenticated, IEnumerable<string> permissionNames)
        {
            var testUser = ServiceProvider.GetRequiredService<TestUser>();

            testUser.IsAuthenticated = isAuthenticated;

            var roleId =  Guid.NewGuid();
            var testRole = new Role
            {
                Id = roleId,
                ExternalId = "test_role",
                Name = "TestRole",
                RolePermissions = permissionNames.Select(permissionName => new RolePermission { PermissionId = _allPermissions.Single(permission => permission.Name == permissionName).Id }).ToList()
            };
            await AddAsExistingEntityAsync(testRole);

            var userId = Guid.NewGuid();
            testUser.User = new User
            {
                Id = userId,
                ExternalId = string.Concat("auth0|", userId.ToString().ToLower()),
                Email = "test@test.com",
                FamilyName = "TestUser",
                Fullname = "TestUser",
                UserRoles = new List<UserRole>() { new() { RoleId = roleId } }
            };
            await AddAsExistingEntityAsync(testUser.User);
        }

        protected async Task AddAsExistingEntityAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            await using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.Set<TEntity>().Add(entity);
            await applicationDbContext.SaveChangesAsync();
        }

        protected async Task AddAsExistingEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            await using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.Set<TEntity>().AddRange(entities);
            await applicationDbContext.SaveChangesAsync();
        }

        protected async Task<TEntity> FindExistingEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return (await FindExistingEntitiesAsync(predicate)).SingleOrDefault();
        }

        protected async Task<IEnumerable<TEntity>> FindExistingEntitiesAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            await using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await applicationDbContext
                .Set<TEntity>()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        protected static void WithRetry(Action assertion, TimeSpan timeout, TimeSpan pollInterval, string because = "",
            params object[] becauseArgs)
        {
            assertion.Should().NotThrowAfter(timeout, pollInterval, because, becauseArgs);
        }
    }
}