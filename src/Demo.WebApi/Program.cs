using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Demo.Application;
using Demo.Application.Shared.Interfaces;
using Demo.Common;
using Demo.Domain;
using Demo.Infrastructure;
using Demo.Infrastructure.Settings;
using Demo.Infrastructure.SignalR;
using Demo.WebApi.Extensions;
using Demo.WebApi.Middleware;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var environmentName = builder.Environment.EnvironmentName;

if (environmentName.Equals(
        "LocalIntegrationTest",
        StringComparison.OrdinalIgnoreCase))
{
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
}

var environmentSettings = new EnvironmentSettings();
builder.Configuration.Bind(environmentSettings);
builder.Services.AddSingleton(environmentSettings);

builder.Host.UseSerilog((_, config) => config
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Environment", environmentName)
    .WriteTo.Debug()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(environmentSettings.ElasticSearch.Uri))
    {
        ModifyConnectionSettings = x => x.BasicAuthentication(environmentSettings.ElasticSearch.Username, environmentSettings.ElasticSearch.Password),
        AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        IndexFormat =
            $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    })
    .ReadFrom.Configuration(builder.Configuration)
);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocument(config =>
{
    config.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
    config.AddSecurity("Bearer", Enumerable.Empty<string>(),
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Example value: Bearer {token}"
        }
    );
});

builder.Services.AddCors(o => o.AddDefaultPolicy(corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins("http://localhost:4401", "http://localhost:5500")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = environmentSettings.Auth0.Domain;
    options.Audience = environmentSettings.Auth0.Audience;

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/hub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEventHubContext, SignalrHubContext>();

builder.Services.AddLogging();

if (!string.IsNullOrEmpty(environmentSettings.Redis.Host))
{
    var redisConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { { environmentSettings.Redis.Host, environmentSettings.Redis.Port } },
        Password = environmentSettings.Redis.Password
    };

    builder.Services.AddSignalR()
        .AddStackExchangeRedis(redisConfigurationOptions.ToString(true),
            options => { options.Configuration.ChannelPrefix = "SignalrHub"; });
}
else
{
    builder.Services.AddSignalR();
}

builder.Services.AddControllersWithViews();
builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{
    options.FileProviders.Clear();
    options.FileProviders.Add(new PhysicalFileProvider(AppContext.BaseDirectory));
});
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var healthChecksBuilder = builder.Services.AddHealthChecks()
    .AddRabbitMQ(
        rabbitConnectionString:
        $"amqp://{HttpUtility.UrlEncode(environmentSettings.RabbitMq.Username)}:{HttpUtility.UrlEncode(environmentSettings.RabbitMq.Password)}@{environmentSettings.RabbitMq.Host}/%2F")
    .AddNpgSql(environmentSettings.Postgres.ConnectionString)
    .AddSignalRHub($"{environmentSettings.WebApiBaseUrl}/hub") // TODO
    .AddElasticsearch(options =>
    {
        options
            .UseServer(environmentSettings.ElasticSearch.Uri)
            .UseBasicAuthentication(environmentSettings.ElasticSearch.Username, environmentSettings.ElasticSearch.Password);
    });

if (!string.IsNullOrEmpty(environmentSettings.Redis.Host))
{
    var redisConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { { environmentSettings.Redis.Host, environmentSettings.Redis.Port } },
        Password = environmentSettings.Redis.Password
    };
    healthChecksBuilder.AddRedis(redisConfigurationOptions.ToString(true));
}

builder.Services
    .AddHealthChecksUI(setupSettings: settings =>
    {
        settings.SetEvaluationTimeInSeconds(60);
        settings.AddHealthCheckEndpoint("hc", "/hc");
    })
    .AddInMemoryStorage();

builder.Services.AddApplication();
builder.Services.AddDomain();
builder.Services.AddInfrastructure(environmentSettings);
builder.Services.AddCommon();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsDockerDev())
{
    app.UseDeveloperExceptionPage();
}

if (!app.Environment.IsProduction())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

if (!app.Environment.IsDockerDev())
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors();

app.UseAuthentication();

app.UseExceptionHandler(x => x.Run(GlobalExceptionHandler.Handle(app.Environment)));
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<CurrentUserIdMiddleware>();

app.UseAuthorization();

app.UseHealthChecks("/hc",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
);

app.UseHealthChecksUI(o =>
{
    o.ApiPath = "/hc-api";
    o.UIPath = "/hc-ui";
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalrHub>("/hub");
});

app.MigrateDatabase();
app.Run();
