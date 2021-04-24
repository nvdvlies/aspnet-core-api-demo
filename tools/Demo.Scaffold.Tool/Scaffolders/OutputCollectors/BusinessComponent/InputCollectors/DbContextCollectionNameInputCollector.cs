using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.InputCollectors
{
    internal class DbContextCollectionNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var collectionName = Prompt.GetString("What is the name of the DbContext collection?");

            context.Variables.Set(Constants.CollectionName, collectionName);
        }
    }
}
