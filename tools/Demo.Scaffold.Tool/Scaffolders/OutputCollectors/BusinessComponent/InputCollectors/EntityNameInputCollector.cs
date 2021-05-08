using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.InputCollectors
{
    internal class EntityNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var entityName = AnsiConsole.Ask<string>("What is the name of the entity?");

            context.Variables.Set(Constants.EntityName, entityName);
        }
    }
}
