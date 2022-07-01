using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class CreateDomainEntityInterfaceOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                Path.Combine(context.GetEntityDirectory(entityName), "Interfaces"),
                $"I{entityName}DomainEntity.cs",
                GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.%ENTITY%.Interfaces
{
    public interface I%ENTITY%DomainEntity : IDomainEntity<%ENTITY%>
    {
    }
}
";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}