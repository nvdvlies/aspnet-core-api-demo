using Demo.SignalrTypescript.Generator.Models;
using Demo.SignalrTypescript.Generator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Demo.SignalrTypescript.Generator
{
    class Program
    {
        public static void Main(string[] args)
        {
            using var host = new HostBuilder()
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile("appsettings.user.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();

                    var appSettings = new AppSettings();
                    configuration.Bind(appSettings);
                    services.AddSingleton(appSettings);

                    services.AddSingleton<SignalrTypescriptGenerator>();
                })
                .Build();

            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var signalrTypescriptGenerator = serviceProvider.GetRequiredService<SignalrTypescriptGenerator>();
            signalrTypescriptGenerator.Run();
        }
    }
}
