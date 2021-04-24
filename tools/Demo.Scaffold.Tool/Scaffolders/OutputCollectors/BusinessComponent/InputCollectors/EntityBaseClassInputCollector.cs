using Demo.Scaffold.Tool.Interfaces;
using McMaster.Extensions.CommandLineUtils;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.InputCollectors
{
    internal class EntityBaseClassInputCollector : IInputCollector
    {
        public void CollectInput(ScaffolderContext context)
        {
            var enableSoftDelete = Prompt.GetYesNo("Enable soft delete?", true);
            if (!enableSoftDelete)
            {
                var enableAuditlogging = Prompt.GetYesNo("Enable audit logging?", true);
                context.Variables.Set(Constants.EnableAuditlogging, enableAuditlogging);
            }

            context.Variables.Set(Constants.EnableSoftDelete, enableSoftDelete);
        }
    }
}
