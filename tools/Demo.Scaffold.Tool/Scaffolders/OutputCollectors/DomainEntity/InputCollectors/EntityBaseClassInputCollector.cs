using Demo.Scaffold.Tool.Interfaces;
using Spectre.Console;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.InputCollectors
{
    internal class EntityBaseClassInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var enableSoftDelete = AnsiConsole.Confirm("Enable soft delete");
            context.Variables.Set(Constants.EnableSoftDelete, enableSoftDelete);

            var enableAuditlogging = AnsiConsole.Confirm("Enable audit logging?");
            context.Variables.Set(Constants.EnableAuditlogging, enableAuditlogging);
        }
    }
}