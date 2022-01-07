using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EntityUpdatedDomainEventOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEventsDirectory(), entityName),
                fileName: $"{entityName}UpdatedDomainEvent.cs",
                content: GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using System;

namespace Demo.Events.%ENTITY%
{
    public class %ENTITY%UpdatedEvent : Event<%ENTITY%UpdatedEvent, %ENTITY%UpdatedEventData>
    {
        public static %ENTITY%UpdatedEvent Create(string correlationId, Guid id, Guid updatedBy)
        {
            var data = new %ENTITY%UpdatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                UpdatedBy = updatedBy
            };
            return new %ENTITY%UpdatedEvent
            {
                Topic = Topics.%ENTITY%,
                Subject = $""%ENTITY%/{data.Id}"",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class %ENTITY%UpdatedEventData : IEventData
    {
        public string EventDataVersion => ""1.0"";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
