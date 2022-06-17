using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EntityDeletedEventHandlerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);
            var collectionName = context.Variables.Get<string>(Constants.CollectionName);

            changes.Add(new CreateNewClass(
                Path.Combine(context.GetApplicationDirectory(), collectionName, "Events", $"{entityName}Deleted"),
                $"{entityName}DeletedEventHandler.cs",
                GetTemplate(collectionName, entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string collectionName, string entityName)
        {
            var code = @"
using Demo.Application.Shared.Interfaces;
using Demo.Events.%ENTITY%;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.%COLLECTIONNAME%.Events.%ENTITY%Deleted
{
    public class %ENTITY%DeletedEventHandler : INotificationHandler<%ENTITY%DeletedEvent>
    {
        private readonly IEventHubContext _eventHubContext;

        public %ENTITY%DeletedEventHandler(IEventHubContext eventHubContext)
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(%ENTITY%DeletedEvent @event, CancellationToken cancellationToken)
        {
            await _eventHubContext.All.%ENTITY%Deleted(@event.Data.Id, @event.Data.DeletedBy);
        }
    }
}";
            code = code.Replace("%COLLECTIONNAME%", collectionName);
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}