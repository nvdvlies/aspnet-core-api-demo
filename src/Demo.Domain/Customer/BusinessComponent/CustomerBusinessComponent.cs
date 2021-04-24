using Demo.Common.Interfaces;
using Demo.Domain.Customer.BusinessComponent.Interfaces;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Demo.Domain.Customer.BusinessComponent
{
    internal class CustomerBusinessComponent : BusinessComponent<Customer>, ICustomerBusinessComponent
    {
        public CustomerBusinessComponent(
            ILogger<CustomerBusinessComponent> logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<Customer> dbCommand,
            IEnumerable<IDefaultValuesSetter<Customer>> defaultValuesSetters,
            IEnumerable<IValidator<Customer>> validators,
            IEnumerable<IBeforeCreate<Customer>> beforeCreateHooks,
            IEnumerable<IAfterCreate<Customer>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<Customer>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<Customer>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<Customer>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<Customer>> afterDeleteHooks,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            IJsonService<Customer> jsonService,
            IAuditlog<Customer> auditlog
        )
            : base(logger, currentUser, dateTime, dbCommand, defaultValuesSetters, validators, beforeCreateHooks, afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks, publishDomainEventAfterCommitQueue, jsonService, auditlog)
        {
        }
    }
}
