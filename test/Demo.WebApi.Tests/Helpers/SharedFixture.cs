using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using AutoFixture;
using Demo.Domain.Role;
using Demo.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using Xunit;

namespace Demo.WebApi.Tests.Helpers;

public class SharedFixture : ICollectionFixture<CustomWebApplicationFactory>
{


    public readonly Checkpoint Checkpoint = new()
    {
        SchemasToInclude = new[] { "demo" },
        TablesToIgnore = new[] { new Table(nameof(Permission)), new Table(nameof(PermissionGroup)) },
        WithReseed = true,
        DbAdapter = DbAdapter.Postgres
    };

    internal readonly EnvironmentSettings EnvironmentSettings;
    internal readonly HttpClient Client;
    internal readonly CustomWebApplicationFactory Factory;
    internal readonly HubConnection HubConnection;
    internal readonly Fixture AutoFixture;
    internal readonly string AccessToken;

    public SharedFixture()
    {
        Factory = new CustomWebApplicationFactory();

        using var scope = Factory.Services.CreateScope();
        EnvironmentSettings = scope.ServiceProvider.GetRequiredService<EnvironmentSettings>();

        AccessToken = GetAccessToken().Result;

        AutoFixture = new Fixture();
        AutoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => AutoFixture.Behaviors.Remove(b));
        AutoFixture.Behaviors.Add(new OmitOnRecursionBehavior());

        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        HubConnection = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl("http://localhost/hub",
                o =>
                {
                    o.AccessTokenProvider = AccessTokenProvider;
                    o.HttpMessageHandlerFactory = _ => Factory.Server.CreateHandler();
                })
            .Build();
        HubConnection.StartAsync().Wait();
    }

    private async Task<string> GetAccessToken()
    {
        var auth0Client = new AuthenticationApiClient(EnvironmentSettings.Auth0.IntegrationTestUser.Domain);
        var tokenRequest = new ClientCredentialsTokenRequest()
        {
            ClientId = EnvironmentSettings.Auth0.IntegrationTestUser.ClientId,
            ClientSecret = EnvironmentSettings.Auth0.IntegrationTestUser.ClientSecret, // TODO
            Audience = EnvironmentSettings.Auth0.Audience
        };
        var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);

        return tokenResponse.AccessToken;
    }

    private Task<string> AccessTokenProvider()
    {
        return Task.FromResult(AccessToken);
    }

}
