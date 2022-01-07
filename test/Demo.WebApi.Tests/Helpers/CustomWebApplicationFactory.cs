using Demo.Infrastructure.Events;
using Demo.Infrastructure.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.WebApi.Tests.Helpers
{

    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        internal IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                    .AddJsonFile("testsettings.json")
                    .Build();

                config.AddConfiguration(Configuration);
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IAuthorizationHandler, AllowUnauthorizedAuthorizationHandler>();

                services.AddTransient<IEventPublisher, FakeEventPublisher>();
                services.AddTransient<IMessageSender, FakeMessageSender>();
            });
        }
    }
}
