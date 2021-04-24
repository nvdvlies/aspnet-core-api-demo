using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.OutputCollectors
{
    internal class CreateBusinessComponentInterfaceOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEntityDirectory(entityName), "BusinessComponent", "Interfaces"),
                fileName: $"I{entityName}BusinessComponent.cs",
                content: GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.%ENTITY%.BusinessComponent.Interfaces
{
    public interface I%ENTITY%BusinessComponent : IBusinessComponent<%ENTITY%>
    {
    }
}
";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
