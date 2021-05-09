using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;
using System;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Query.InputCollectors
{
    internal class QueryEndpointTypeInputCollector : IInputCollector
    {
        private const string Search = "Search (GET api/[[controller]]/)";
        private const string GetById = "GetById (GET api/[[controller]]/{id})";
        private const string SubEndpoint = "SubEndpoint (GET api/[[controller]]/{id}/[[name]])";

        public void CollectInput(ScaffolderContext context)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which type of endpoint would you like to add?")
                    .AddChoices(new[] {
                        Search,
                        GetById,
                        SubEndpoint,
                    }));

            var queryEndpointType = option switch
            {
                Search => QueryEndpointTypes.Search,
                GetById => QueryEndpointTypes.GetById,
                SubEndpoint => QueryEndpointTypes.SubEndpoint,
                _ => throw new Exception($"Invalid option {option}"),
            };

            context.Variables.Set(Constants.QueryEndpointType, queryEndpointType);
        }
    }
}
