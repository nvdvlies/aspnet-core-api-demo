using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.InputCollectors
{
    internal class DbContextCollectionNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var collectionName = AnsiConsole.Ask<string>("What is the name of the DbContext collection?");

            context.Variables.Set(Constants.CollectionName, collectionName);
        }
    }
}