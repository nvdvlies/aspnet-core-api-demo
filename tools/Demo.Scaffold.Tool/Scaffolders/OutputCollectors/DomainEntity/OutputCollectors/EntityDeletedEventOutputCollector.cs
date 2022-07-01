using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EntityDeletedEventOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                Path.Combine(context.GetEventsDirectory(), entityName),
                $"{entityName}DeletedEvent.cs",
                GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using System;

namespace Demo.Events.%ENTITY%
{
    public class %ENTITY%DeletedEvent : Event<%ENTITY%DeletedEvent, %ENTITY%DeletedEventData>
    {
        public static %ENTITY%DeletedEvent Create(Guid correlationId, Guid id, Guid deletedBy)
        {
            var data = new %ENTITY%DeletedEventData
            {
                CorrelationId = correlationId,
                Id = id,
                DeletedBy = deletedBy
            };
            return new %ENTITY%DeletedEvent
            {
                Topic = Topics.%ENTITY%,
                Subject = $""%ENTITY%/{data.Id}"",
                Data = data,
                DataVersion = data.EventDataVersion,
                CorrelationId = correlationId
            };
        }
    }

    public class %ENTITY%DeletedEventData : IEventData
    {
        public string EventDataVersion => ""1.0"";
        public Guid CorrelationId { get; set; }

        public Guid Id { get; set; }
        public Guid DeletedBy { get; set; }
    }
}";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}