using System.IO;
using Demo.Scaffold.Tool.Models;
using Demo.Scaffold.Tool.Scaffolders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Scaffold.Tool
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using var host = new HostBuilder()
                .ConfigureLogging((context, builder) => { builder.AddConsole(); })
                .ConfigureServices((context, services) =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile(Constants.UserSettingsFileName, true)
                        .Build();

                    var appSettings = new AppSettings();
                    configuration.Bind(appSettings);
                    services.AddSingleton(appSettings);

                    services.AddSingleton<ScaffolderService>();
                })
                .Build();

            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var scaffolderService = serviceProvider.GetRequiredService<ScaffolderService>();
            scaffolderService.Run();
        }
    }
}