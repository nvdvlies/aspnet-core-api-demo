using System.IO;
using Demo.SignalrTypescript.Generator.Models;
using Demo.SignalrTypescript.Generator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.SignalrTypescript.Generator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using var host = new HostBuilder()
                .ConfigureLogging((_, builder) => { builder.AddConsole(); })
                .ConfigureServices((_, services) =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile("appsettings.user.json", true)
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