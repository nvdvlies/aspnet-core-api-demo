using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EntityCreatedEventOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEventsDirectory(), entityName),
                fileName: $"{entityName}CreatedEvent.cs",
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
    public class %ENTITY%CreatedEvent : Event<%ENTITY%CreatedEvent, %ENTITY%CreatedEventData>
    {
        public static %ENTITY%CreatedEvent Create(Guid correlationId, Guid id, Guid createdBy)
        {
            var data = new %ENTITY%CreatedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                CreatedBy = createdBy
            };
            return new %ENTITY%CreatedEvent
            {
                Topic = Topics.%ENTITY%,
                Subject = $""%ENTITY%/{data.Id}"",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class %ENTITY%CreatedEventData : IEventData
    {
        public string EventDataVersion => ""1.0"";
        public Guid CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
    }
}";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
