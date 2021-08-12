using Demo.Application;
using Demo.Application.Shared.Interfaces;
using Demo.Common.Interfaces;
using Demo.Domain;
using Demo.Infrastructure;
using Demo.WebApi.Middleware;
using Demo.WebApi.Services;
using Demo.WebApi.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, GetNewtonsoftJsonPatchInputFormatter());
            });
            services.AddSwaggerDocument();

            services.AddCors(o => o.AddPolicy("AllowAnyCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

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

                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowAnyCorsPolicy");

            app.UseAuthorization();

            app.UseExceptionHandler(x => x.Run(GlobalExceptionHandler.Handle(env)));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalrHub>("/hub");
            });
        }


        /// <summary>
        /// AddNewtonsoftJson() replaces the System.Text.Json-based input and output formatters used for formatting all JSON content. 
        /// To leave the other formatters unchanged we need to build a dummy ServiceProvider with AddNewtonsoftJson() and grab the 
        /// NewtonsoftJsonPatchInputFormatter input formatter.
        /// </summary>
        /// <returns></returns>
        private static NewtonsoftJsonPatchInputFormatter GetNewtonsoftJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddControllers()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            var inputFormatter = builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();

            return inputFormatter;
        }
    }
}
