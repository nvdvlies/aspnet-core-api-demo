using System.Collections.Generic;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Helpers;
using Demo.Scaffold.Tool.Interfaces;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

internal class CreateAuditloggerOutputCollector : IOutputCollector
{
    public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
    {
        var changes = new List<IChange>();

        context.Variables.TryGet<bool>(Constants.EnableAuditlogging, out var enableAuditlogging);
        if (!enableAuditlogging)
        {
            return changes;
        }

        var entityName = context.Variables.Get<string>(Constants.EntityName);

        changes.Add(new CreateNewClass(
            context.GetAuditloggerDirectory(),
            $"{entityName}Auditlogger.cs",
            GetTemplate(entityName)
        ));

        return changes;
    }

    private static string GetTemplate(string entityName)
    {
        var code = @"
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.%ENTITY%;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging.Shared;
using System.Collections.Generic;

namespace Demo.Infrastructure.Auditlogging
{
    internal class %ENTITY%Auditlogger : AuditloggerBase<%ENTITY%>, IAuditlogger<%ENTITY%>
    {
        public %ENTITY%Auditlogger(            
            ICurrentUser currentUser, 
            IDateTime dateTime,
            IAuditlogDomainEntity auditlogDomainEntity
        ) : base(currentUser, dateTime, auditlogDomainEntity)
        {
        }

        protected override List<AuditlogItem> AuditlogItems(%ENTITY% current, %ENTITY% previous) => 
            new AuditlogBuilder<%ENTITY%>()
                // TODO : .WithProperty(x => x.PropertyName)
                .Build(current, previous);
    }
}
";
        code = code.Replace("%ENTITY%", entityName);
        return code;
    }
}
