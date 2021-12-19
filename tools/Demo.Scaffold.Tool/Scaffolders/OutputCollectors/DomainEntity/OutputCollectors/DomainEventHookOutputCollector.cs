using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class DomainEventHookOutputCollector : IOutputCollector
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
using Demo.Domain.%ENTITY%.DomainEntity.Events;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.%ENTITY%.DomainEntity.Hooks
{
    internal class %ENTITY%CreatedUpdatedDeletedDomainEventHook : IAfterCreate<%ENTITY%>, IAfterUpdate<%ENTITY%>, IAfterDelete<%ENTITY%>
    {
        private readonly ICurrentUser _currentUser;

        public %ENTITY%CreatedUpdatedDeletedDomainEventHook(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<%ENTITY%> context, CancellationToken cancellationToken)
        {
            switch (context.EditMode)
            {
                case EditMode.Create:
                    context.PublishDomainEventAfterCommit(new %ENTITY%CreatedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Update:
                    context.PublishDomainEventAfterCommit(new %ENTITY%UpdatedDomainEvent(context.Entity.Id, _currentUser.Id));
                    break;
                case EditMode.Delete:
                    context.PublishDomainEventAfterCommit(new %ENTITY%DeletedDomainEvent(context.Entity.Id, _currentUser.Id));
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
