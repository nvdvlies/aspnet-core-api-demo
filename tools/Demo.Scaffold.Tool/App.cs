using Demo.Scaffold.Tool.Scaffolders;
using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace Demo.Scaffold.Tool
{
    [Command(
           Name = "dotnet demo scaffold",
           FullName = "dotnet-demo-scaffold",
           Description = "Scaffolds endpoints, handlers, business component, entities"
       )]
    [HelpOption]
    internal class App
    {
        public Task<int> OnExecute(ScaffolderService scaffolderService)
        {
            scaffolderService.Run();

            return Task.FromResult(0);
        }
    }
}
