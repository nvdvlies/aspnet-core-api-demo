using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.OutputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent.InputCollectors;
using Demo.Scaffold.Tool.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.BusinessComponent
{
    internal class BusinessComponentOutputCollector : IOutputCollector
    {
        private List<IInputCollector> _inputCollectors = new List<IInputCollector>()
        {
            new EntityNameInputCollector(),
            new EntityBaseClassInputCollector(),
            new DbContextCollectionNameInputCollector()
        };

        private List<IOutputCollector> _outputCollectors = new List<IOutputCollector>()
        {
            new CreateEntityClassOutputCollector(),
            new CreateBusinessComponentInterfaceOutputCollector(),
            new CreateBusinessComponentOutputCollector(),
            new CreateAuditloggerOutputCollector(),
            new CreateEntityTypeConfigurationOutputCollector(),
            new AddDbSetToDbContextOutputCollector(),
            new AddUsingStatementToDbContextOutputCollector(),
        };

        public IEnumerable<IChange> CollectChanges(ScaffolderContext context)
        {
            foreach (var inputCollector in _inputCollectors)
            {
                inputCollector.CollectInput(context);
            }

            var changes = _outputCollectors
                .Select(x => x.CollectChanges(context))
                .SelectMany(x => x)
                .Where(x => x != null)
                .ToList();

            return changes;
        }
    }
}
