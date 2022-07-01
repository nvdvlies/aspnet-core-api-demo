using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Application;
using Demo.Application.Shared.Interfaces;
using Demo.Common;
using Demo.Domain;
using Demo.Infrastructure;
using Demo.Infrastructure.Settings;
using Demo.Infrastructure.SignalR;
using Demo.WebApi.Auth;
using Demo.WebApi.Extensions;
using Demo.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;

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
                    new OpenApiSecurityScheme
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
                builder.WithOrigins("http://localhost:4401", "http://localhost:5500")
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(Policies.User),
                    policy => policy.Requirements.Add(new HasRoleRequirement(Auth0Roles.User,
                        environmentSettings.Auth0.Domain)));
                options.AddPolicy(nameof(Policies.Admin),
                    policy => policy.Requirements.Add(new HasRoleRequirement(Auth0Roles.Admin,
                        environmentSettings.Auth0.Domain)));
                options.AddPolicy(nameof(Policies.Machine),
                    policy => policy.Requirements.Add(new HasRoleRequirement(Auth0Roles.Machine,
                        environmentSettings.Auth0.Domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasRoleRequirementAuthorizationHandler>();

            services.AddHttpContextAccessor();
            services.AddScoped<IEventHubContext, SignalrHubContext>();

            services.AddLogging();

            services.AddSignalR()
                .AddStackExchangeRedis(environmentSettings.Redis.Connection,
                    options => { options.Configuration.ChannelPrefix = "SignalrHub"; });

            services.AddControllersWithViews();
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(new PhysicalFileProvider(AppContext.BaseDirectory));
            });
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddApplication();
            services.AddDomain();
            services.AddInfrastructure(Configuration, environmentSettings);
            services.AddCommon();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsDockerDev())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsProduction())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            if (!env.IsDockerDev())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandler(x => x.Run(GlobalExceptionHandler.Handle(env)));
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<CurrentUserIdMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalrHub>("/hub");
            });
        }
    }
}