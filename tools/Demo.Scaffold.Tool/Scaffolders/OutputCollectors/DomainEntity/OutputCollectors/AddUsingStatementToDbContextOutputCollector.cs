using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

internal class AddUsingStatementToDbContextOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var entityName = context.Variables.Get<string>(Constants.EntityName);

        changes.Add(new AddUsingStatementToExistingClass(
            context.GetPersistenceDirectory(),
            "ApplicationDbContext.cs",
            $"using Demo.Domain.{entityName};"
        ));

        return changes;
    }
}
