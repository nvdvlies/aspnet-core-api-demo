using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

internal class EntityUpdatedEventHandlerOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var entityName = context.Variables.Get<string>(Constants.EntityName);
        var collectionName = context.Variables.Get<string>(Constants.CollectionName);

        changes.Add(new CreateNewClass(
            Path.Combine(context.GetApplicationDirectory(), collectionName, "Events", $"{entityName}Updated"),
            $"{entityName}UpdatedEventHandler.cs",
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

namespace Demo.Application.%COLLECTIONNAME%.Events.%ENTITY%Updated
{
    public class %ENTITY%UpdatedEventHandler : INotificationHandler<%ENTITY%UpdatedEvent>
    {
        private readonly ILogger<%ENTITY%UpdatedEventHandler> _logger;
        private readonly IEventHubContext _eventHubContext;

        public %ENTITY%UpdatedEventHandler(
            ILogger<%ENTITY%UpdatedEventHandler> logger,
            IEventHubContext eventHubContext
        )
        {
            _eventHubContext = eventHubContext;
        }

        public async Task Handle(%ENTITY%UpdatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($""Handling {nameof(%ENTITY%UpdatedEvent)}"");
            await _eventHubContext.All.%ENTITY%Updated(@event.Data.Id, @event.Data.UpdatedBy);
        }
    }
}";
        code = code.Replace("%COLLECTIONNAME%", collectionName);
        code = code.Replace("%ENTITY%", entityName);
        return code;
    }
}
