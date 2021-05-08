using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;
using System;

namespace Demo.Scaffold.Tool.Scaffolders.InputCollectors
{
    internal class ScaffoldTypeInputCollector : IInputCollector
    {
        private const string BusinessComponent = "BusinessComponent";
        private const string Endpoint = "Endpoint";

        public void CollectInput(ScaffolderContext context)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to scaffold?")
                    .AddChoices(new[] {
                        BusinessComponent,
                        Endpoint,
                    }));

            context.ScaffolderType = option switch
            {
                BusinessComponent => ScaffolderTypes.BusinessComponent,
                Endpoint => ScaffolderTypes.Endpoint,
                _ => throw new Exception($"Invalid option {option}"),
            };
        }
    }
}
