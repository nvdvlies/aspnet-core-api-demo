using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EventHubOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);
            var collectionName = context.Variables.Get<string>(Constants.CollectionName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetApplicationDirectory(), collectionName, "Events"),
                fileName: $"I{entityName}EventHub.cs",
                content: GetTemplate(collectionName, entityName)
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
        Task %ENTITY%Created(Guid id, string createdBy);
        Task %ENTITY%Updated(Guid id, string updatedBy);
        Task %ENTITY%Deleted(Guid id, string deletedBy);
        %SCAFFOLDMARKER%
    }
}";
            code = code.Replace("%COLLECTIONNAME%", collectionName);
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%SCAFFOLDMARKER%", Tool.Constants.ScaffoldMarkerEventHub);
            return code;
        }
    }
}
