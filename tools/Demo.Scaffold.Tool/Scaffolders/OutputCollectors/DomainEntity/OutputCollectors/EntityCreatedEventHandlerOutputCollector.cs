using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EntityCreatedEventHandlerOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);
            var collectionName = context.Variables.Get<string>(Constants.CollectionName);

            changes.Add(new CreateNewClass(
                Path.Combine(context.GetApplicationDirectory(), collectionName, "Events", $"{entityName}Created"),
                $"{entityName}CreatedEventHandler.cs",
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

namespace Demo.Application.%COLLECTIONNAME%.Events.%ENTITY%Created
{
    public class %ENTITY%CreatedEventHandler : INotificationHandler<%ENTITY%CreatedEvent>
    {
        private readonly ILogger<%ENTITY%CreatedEventHandler> _logger;
        private readonly IEventHubContext _eventHubContext;

        public %ENTITY%CreatedEventHandler(
            ILogger<%ENTITY%CreatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(%ENTITY%CreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($""Handling {nameof(%ENTITY%CreatedEvent)}"");
            await _eventHubContext.All.%ENTITY%Created(@event.Data.Id, @event.Data.CreatedBy);
        }
    }
}";
            code = code.Replace("%COLLECTIONNAME%", collectionName);
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}