using System.Collections.Generic;
using System.IO;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

internal class EventHookOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var entityName = context.Variables.Get<string>(Constants.EntityName);

        changes.Add(new CreateNewClass(
            Path.Combine(context.GetEntityDirectory(entityName), "Hooks"),
            $"{entityName}CreatedUpdatedDeletedDomainEventHook.cs",
            GetTemplate(entityName)
        ));

        return changes;
    }

    private static string GetTemplate(string entityName)
    {
        var code = @"
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Events.%ENTITY%;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.%ENTITY%.Hooks
{
    internal class %ENTITY%CreatedUpdatedDeletedEventHook : IAfterCreate<%ENTITY%>, IAfterUpdate<%ENTITY%>, IAfterDelete<%ENTITY%>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public %ENTITY%CreatedUpdatedDeletedEventHook(
            ICurrentUser currentUser,
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _currentUser = currentUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task ExecuteAsync(HookType type, IDomainEntityContext<%ENTITY%> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    await context.AddEventAsync(%ENTITY%CreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Update:
                    await context.AddEventAsync(%ENTITY%UpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Delete:
                    await context.AddEventAsync(%ENTITY%DeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
            }
        }
    }
}";
        code = code.Replace("%ENTITY%", entityName);
        return code;
    }
}
