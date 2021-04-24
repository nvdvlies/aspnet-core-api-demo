using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.OutputCollectors
{
    internal class CreateEntityClassOutputCollector : IOutputCollector
    {
        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            var changes = new List<IChange>();

            var entityName = context.Variables.Get<string>(Constants.EntityName);
            var enableSoftDelete = context.Variables.Get<bool>(Constants.EnableSoftDelete);
            var enableAuditlogging = context.Variables.Get<bool>(Constants.EnableAuditlogging);

            changes.Add(new CreateNewClass(
                directory: context.GetEntityDirectory(entityName),
                fileName: $"{entityName}.cs",
                content: GetTemplate(entityName, enableSoftDelete, enableAuditlogging)
            ));

            return changes;
        }

        private static string GetTemplate(string entityName, bool enableSoftDelete, bool enableAuditlogging)
        {
            var code = @"
using Demo.Domain.Shared.Entities;
using System;

namespace Demo.Domain.%ENTITY%
{
    public partial class %ENTITY% : %BASE_CLASS%, IQueryableEntity
    {
    }
}
";
            code = code.Replace("%ENTITY%", entityName);
            code = code.Replace("%BASE_CLASS%", enableSoftDelete ? "SoftDeleteEntity" : enableAuditlogging ? "AuditableEntity" : "Entity");
            return code;
        }
    }
}
