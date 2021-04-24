using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.OutputCollectors
{
    internal class CreateBusinessComponentOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);

            changes.Add(new CreateNewClass(
                directory: Path.Combine(context.GetEntityDirectory(entityName), "BusinessComponent"),
                fileName: $"{entityName}BusinessComponent.cs",
                content: GetTemplate(entityName)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName)
        {
            var code = @"
using Demo.Common.Interfaces;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.%ENTITY%.BusinessComponent.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Demo.Domain.%ENTITY%.BusinessComponent
{
    internal class %ENTITY%BusinessComponent : BusinessComponent<%ENTITY%>, I%ENTITY%BusinessComponent
    {
        public %ENTITY%BusinessComponent(
            ILogger<%ENTITY%BusinessComponent> logger,
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
            IDomainEventQueue domainEventQueue,
            IJsonService<%ENTITY%> jsonService,
            IAuditlog<%ENTITY%> auditlog
        ) 
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, domainEventQueue, jsonService, auditlog)
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
