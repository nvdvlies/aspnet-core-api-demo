using System;
using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.Endpoint.OutputCollectors.Command.InputCollectors
{
    internal class CommandEndpointTypeInputCollector : IInputCollector
    {
        private const string Create = "Create (POST api/[[controller]]/)";
        private const string Update = "Update (PUT api/[[controller]]/)";
        private const string Delete = "Delete (DELETE api/[[controller]]/)";
        private const string CreateSubEndpoint = "Create SubEndpoint (POST api/[[controller]]/{id}/[[name]])";
        private const string UpdateSubEndpoint = "Update SubEndpoint (PUT api/[[controller]]/{id}/[[name]])";
        private const string DeleteSubEndpoint = "Delete SubEndpoint (DELETE api/[[controller]]/{id}/[[name]])";

        public void CollectInput(ScaffolderContext context)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which type of endpoint would you like to add?")
                    .AddChoices(Create, Update, Delete, CreateSubEndpoint, UpdateSubEndpoint, DeleteSubEndpoint));

            var commandEndpointTypes = option switch
            {
                Create => CommandEndpointTypes.Create,
                Update => CommandEndpointTypes.Update,
                Delete => CommandEndpointTypes.Delete,
                CreateSubEndpoint => CommandEndpointTypes.CreateSubEndpoint,
                UpdateSubEndpoint => CommandEndpointTypes.UpdateSubEndpoint,
                DeleteSubEndpoint => CommandEndpointTypes.DeleteSubEndpoint,
                _ => throw new Exception($"Invalid option {option}")
            };

            context.Variables.Set(Constants.CommandEndpointType, commandEndpointTypes);
        }
    }
}
