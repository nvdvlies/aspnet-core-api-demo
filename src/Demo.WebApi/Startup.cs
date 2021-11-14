using Demo.Application;
using Demo.Application.Shared.Interfaces;
using Demo.Common.Interfaces;
using Demo.Domain;
using Demo.Infrastructure;
using Demo.Infrastructure.Settings;
using Demo.WebApi.Auth0;
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
            services.AddSwaggerDocument();

            services.AddCors(o => o.AddPolicy("AllowAnyCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4201")
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
                options.AddPolicy(nameof(Auth0Scopes.User), policy => policy.Requirements.Add(new HasScopeRequirement(Auth0Scopes.User, environmentSettings.Auth0.Domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddScoped<IEventHubContext, SignalrHubContext>();

            services.AddLogging();

            services.AddApplication();
            services.AddDomain();
            services.AddInfrastructure(Configuration);

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
            app.UseCors("AllowAnyCorsPolicy");

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
