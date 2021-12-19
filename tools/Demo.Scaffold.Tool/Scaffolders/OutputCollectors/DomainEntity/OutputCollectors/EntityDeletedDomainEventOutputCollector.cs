using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EntityDeletedDomainEventOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEntityDirectory(entityName), "DomainEntity", "Events"),
                fileName: $"{entityName}DeletedDomainEvent.cs",
                content: GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.%ENTITY%.DomainEntity.Events
{
    public class %ENTITY%DeletedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }

        public %ENTITY%DeletedDomainEvent(Guid id, Guid deletedBy)
        {
            Id = id;
            DeletedBy = deletedBy;
        }
    }
}";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
