using Demo.Scaffold.Tool.Models;
using Demo.Scaffold.Tool.Scaffolders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Demo.Scaffold.Tool
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new HostBuilder()
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                })
                .ConfigureServices((context, services) => {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile(Constants.UserSettingsFileName, optional: true)
                        .Build();

                    var appSettings = new AppSettings();
                    configuration.Bind(appSettings);
                    services.AddSingleton(appSettings);

                    services.AddSingleton<ScaffolderService>();
                })
                .RunCommandLineApplicationAsync<App>(args);
        }
    }
}
