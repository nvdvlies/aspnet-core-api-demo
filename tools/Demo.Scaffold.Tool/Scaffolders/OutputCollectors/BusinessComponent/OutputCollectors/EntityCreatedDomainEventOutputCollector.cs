using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.OutputCollectors
{
    internal class EntityCreatedDomainEventOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEntityDirectory(entityName), "BusinessComponent", "Events"),
                fileName: $"{entityName}CreatedDomainEvent.cs",
                content: GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.%ENTITY%.BusinessComponent.Events
{
    public class %ENTITY%CreatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }

        public %ENTITY%CreatedDomainEvent(Guid id, Guid createdBy)
        {
            Id = id;
            CreatedBy = createdBy;
        }
    }
}";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
