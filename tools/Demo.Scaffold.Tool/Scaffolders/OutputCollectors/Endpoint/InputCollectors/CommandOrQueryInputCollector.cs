using System;
using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.InputCollectors;

internal class CommandOrQueryInputCollector : IInputCollector
{
    private const string Command = "Command";
    private const string Query = "Query";

    public void CollectInput(ScaffolderContext context)
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Command or query?")
                .AddChoices(Command, Query));

        switch (option)
        {
            case Command:
                context.Variables.Set(Constants.EndpointType, EndpointTypes.Command);
                break;
            case Query:
                context.Variables.Set(Constants.EndpointType, EndpointTypes.Query);
                break;
            default:
                throw new NotSupportedException($"Invalid endpoint type '{option}'");
        }
    }
}
