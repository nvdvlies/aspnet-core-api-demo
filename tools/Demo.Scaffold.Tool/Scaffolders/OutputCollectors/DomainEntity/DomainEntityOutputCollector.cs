using System.Collections.Generic;
using System.Linq;
using Demo.Scaffold.Tool.Changes;
using Demo.Scaffold.Tool.Interfaces;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.InputCollectors;
using Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity.OutputCollectors;

namespace Demo.Scaffold.Tool.Scaffolders.OutputCollectors.DomainEntity
{
    internal class DomainEntityOutputCollector : IOutputCollector
    {
        private readonly List<IInputCollector> _inputCollectors = new()
        {
            new EntityNameInputCollector(),
            new EntityBaseClassInputCollector(),
            new DbContextCollectionNameInputCollector()
        };

        private readonly List<IOutputCollector> _outputCollectors = new()
        {
            new CreateEntityClassOutputCollector(),
            new CreateDomainEntityInterfaceOutputCollector(),
            new CreateDomainEntityOutputCollector(),
            new CreateAuditloggerOutputCollector(),
            new CreateEntityTypeConfigurationOutputCollector(),
            new AddDbSetToDbContextOutputCollector(),
            new AddUsingStatementToDbContextOutputCollector(),
            new EntityCreatedEventOutputCollector(),
            new EntityUpdatedEventOutputCollector(),
            new EntityDeletedEventOutputCollector(),
            new EventHookOutputCollector(),
            new EventHubOutputCollector(),
            new AddEventHubToIEventHubOutputCollector(),
            new EntityCreatedEventHandlerOutputCollector(),
            new EntityUpdatedEventHandlerOutputCollector(),
            new EntityDeletedEventHandlerOutputCollector()
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
