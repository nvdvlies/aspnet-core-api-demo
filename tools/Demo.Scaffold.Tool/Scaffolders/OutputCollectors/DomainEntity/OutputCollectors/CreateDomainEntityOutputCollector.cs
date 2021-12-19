using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors
{
    internal class CreateDomainEntityOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEntityDirectory(entityName), "DomainEntity"),
                fileName: $"{entityName}DomainEntity.cs",
                content: GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.%ENTITY%.DomainEntity.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Demo.Domain.%ENTITY%.DomainEntity
{
    internal class %ENTITY%DomainEntity : DomainEntity<%ENTITY%>, I%ENTITY%DomainEntity
    {
        public %ENTITY%DomainEntity(
            ILogger<%ENTITY%DomainEntity> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<%ENTITY%> dbCommand, 
            IEnumerable<IDefaultValuesSetter<%ENTITY%>> defaultValuesSetters, 
            IEnumerable<IValidator<%ENTITY%>> validators, 
            IEnumerable<IBeforeCreate<%ENTITY%>> beforeCreateHooks, 
            IEnumerable<IAfterCreate<%ENTITY%>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<%ENTITY%>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<%ENTITY%>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<%ENTITY%>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<%ENTITY%>> afterDeleteHooks,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            IJsonService<%ENTITY%> jsonService,
            IAuditlog<%ENTITY%> auditlog
        ) 
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, publishDomainEventAfterCommitQueue, jsonService, auditlog)
        {
        }
    }
}
";
            code = code.Replace("%ENTITY%", entityName);
            return code;
        }
    }
}
