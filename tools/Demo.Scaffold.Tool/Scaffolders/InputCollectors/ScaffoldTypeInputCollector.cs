using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace Demo.Scaffold.Tool.Scaffolders.InputCollectors
{
    internal class ScaffoldTypeInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var option = Prompt.GetInt("What would you like to scaffold? (1 = BusinessComponent, 2 = Endpoint):");

            context.ScaffolderType = option switch
            {
                1 => ScaffolderTypes.BusinessComponent,
                2 => ScaffolderTypes.Endpoint,
                _ => throw new Exception($"Invalid option {option}"),
            };
        }
    }
}
