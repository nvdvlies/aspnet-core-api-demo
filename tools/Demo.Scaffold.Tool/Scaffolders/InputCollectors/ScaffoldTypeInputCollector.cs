using System;
using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.InputCollectors;

internal class ScaffoldTypeInputCollector : IInputCollector
{
    private const string DomainEntity = "DomainEntity";
    private const string Endpoint = "Endpoint";

    public void CollectInput(ScaffolderContext context)
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to scaffold?")
                .AddChoices(DomainEntity, Endpoint));

        context.ScaffolderType = option switch
        {
            DomainEntity => ScaffolderTypes.DomainEntity,
            Endpoint => ScaffolderTypes.Endpoint,
            _ => throw new Exception($"Invalid option {option}")
        };
    }
}
