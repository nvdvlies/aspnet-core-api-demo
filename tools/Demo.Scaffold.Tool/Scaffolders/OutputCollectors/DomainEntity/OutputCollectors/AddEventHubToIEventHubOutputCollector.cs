using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

internal class AddEventHubToIEventHubOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var entityName = context.Variables.Get<string>(Constants.EntityName);

        changes.Add(new UpdateExistingClassAtMarker(
            Path.Combine(context.GetApplicationDirectory(), "Shared", "Interfaces"),
            "IEventHub.cs",
            Tool.Constants.ScaffoldMarkerEventHubInterface,
            GetTemplate(entityName)
        ));

        return changes;
    }

    private static string GetTemplate(string entityName)
    {
        var code = @",
        I%ENTITY%EventHub";

        code = code.Replace("%ENTITY%", entityName);
        return code;
    }
}
