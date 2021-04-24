using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.InputCollectors
{
    internal class EntityNameInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var entityName = Prompt.GetString("What is the name of the entity?");

            context.Variables.Set(Constants.EntityName, entityName);
        }
    }
}
