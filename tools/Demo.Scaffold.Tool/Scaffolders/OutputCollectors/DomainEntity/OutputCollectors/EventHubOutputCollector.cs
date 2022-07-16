using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

internal class EventHubOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var entityName = context.Variables.Get<string>(Constants.EntityName);
        var collectionName = context.Variables.Get<string>(Constants.CollectionName);

        changes.Add(new CreateNewClass(
            Path.Combine(context.GetApplicationDirectory(), collectionName, "Events"),
            $"I{entityName}EventHub.cs",
            GetTemplate(collectionName, entityName)
        ));

        return changes;
    }

    private static string GetTemplate(string collectionName, string entityName)
    {
        var code = @"
using System;
using System.Threading.Tasks;

namespace Demo.Application.%COLLECTIONNAME%.Events
{
    public interface I%ENTITY%EventHub
    {
        Task %ENTITY%Created(Guid id, Guid createdBy);
        Task %ENTITY%Updated(Guid id, Guid updatedBy);
        Task %ENTITY%Deleted(Guid id, Guid deletedBy);
        %SCAFFOLDMARKER%
    }
}";
        code = code.Replace("%COLLECTIONNAME%", collectionName);
        code = code.Replace("%ENTITY%", entityName);
        code = code.Replace("%SCAFFOLDMARKER%", Tool.Constants.ScaffoldMarkerEventHub);
        return code;
    }
}
