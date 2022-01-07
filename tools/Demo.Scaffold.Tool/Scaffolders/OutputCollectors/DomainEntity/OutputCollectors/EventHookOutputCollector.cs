using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class EventHookOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEntityDirectory(entityName), "DomainEntity", "Hooks"),
                fileName: $"{entityName}CreatedUpdatedDeletedDomainEventHook.cs",
                content: GetTemplate(entityName)
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

        public Task ExecuteAsync(HookType type, IDomainEntityContext<%ENTITY%> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.AddEventAsync(%ENTITY%CreatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Update:
                    context.AddEventAsync(%ENTITY%UpdatedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
                case EditMode.Delete:
                    context.AddEventAsync(%ENTITY%DeletedEvent.Create(_correlationIdProvider.Id, context.Entity.Id, _currentUser.Id), cancellationToken);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
