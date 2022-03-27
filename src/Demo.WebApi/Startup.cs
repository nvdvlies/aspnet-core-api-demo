using Demo.Application;
using Demo.Application.Shared.Interfaces;
using Demo.Common;
using Demo.Common.Interfaces;
using Demo.Domain;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure;
using Demo.Infrastructure.Settings;
using Demo.WebApi.Auth;
using Demo.WebApi.Middleware;
using Demo.WebApi.Services;
using Demo.WebApi.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Linq;

namespace Demo.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var environmentSettings = new EnvironmentSettings();
            Configuration.Bind(environmentSettings);
            services.AddSingleton(environmentSettings);

            services.AddControllers();
            services.AddSwaggerDocument(config =>
            {
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
                config.AddSecurity("Bearer", Enumerable.Empty<string>(),
                    new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Example value: Bearer {token}"
                    }
                );
            });

            services.AddCors(o => o.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:4401")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = environmentSettings.Auth0.Domain;
                options.Audience = environmentSettings.Auth0.Audience;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(Policies.User), policy => policy.Requirements.Add(new HasRoleRequirement(Auth0Roles.User, environmentSettings.Auth0.Domain)));
                options.AddPolicy(nameof(Policies.Admin), policy => policy.Requirements.Add(new HasRoleRequirement(Auth0Roles.Admin, environmentSettings.Auth0.Domain)));
                options.AddPolicy(nameof(Policies.Machine), policy => policy.Requirements.Add(new HasRoleRequirement(Auth0Roles.Machine, environmentSettings.Auth0.Domain)));
            });

            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<IAuthorizationHandler, HasRoleRequirementAuthorizationHandler>();

            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddScoped<IEventHubContext, SignalrHubContext>();

            services.AddLogging();

            services.AddApplication();
            services.AddDomain();
            services.AddInfrastructure(Configuration, environmentSettings);
            services.AddCommon();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsProduction())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandler(x => x.Run(GlobalExceptionHandler.Handle(env)));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalrHub>("/hub");
            });
        }
    }
}
