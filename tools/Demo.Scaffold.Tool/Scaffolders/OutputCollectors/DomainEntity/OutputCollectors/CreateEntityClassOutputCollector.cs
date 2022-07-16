using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

internal class CreateEntityClassOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        var entityName = context.Variables.Get<string>(Constants.EntityName);
        var enableSoftDelete = context.Variables.Get<bool>(Constants.EnableSoftDelete);
        context.Variables.TryGet<bool>(Constants.EnableAuditlogging, out var enableAuditlogging);

        changes.Add(new CreateNewClass(
            context.GetEntityDirectory(entityName),
            $"{entityName}.cs",
            GetTemplate(entityName, enableSoftDelete, enableAuditlogging)
        ));

        return changes;
    }

    private static string GetTemplate(string entityName, bool enableSoftDelete, bool enableAuditlogging)
    {
        var code = @"
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using System;

namespace Demo.Domain.%ENTITY%
{
    public partial class %ENTITY% : %BASE_CLASS%, IQueryableEntity
    {
    }
}
";
        code = code.Replace("%ENTITY%", entityName);
        var baseClass = enableSoftDelete
            ? "SoftDeleteEntity"
            : enableAuditlogging
                ? "AuditableEntity"
                : "Entity";
        code = code.Replace("%BASE_CLASS%", baseClass);
        return code;
    }
}
