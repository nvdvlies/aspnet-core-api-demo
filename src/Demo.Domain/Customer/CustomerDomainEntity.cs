using System;
using System.Collections.Generic;
using Demo.Common.Interfaces;
using Demo.Domain.Customer.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Customer
{
    internal class CustomerDomainEntity : DomainEntity<Customer>, ICustomerDomainEntity
    {
        public CustomerDomainEntity(
            ILogger<CustomerDomainEntity> logger,
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IDbCommand<Customer> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<Customer>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<Customer>>> validators,
            Lazy<IEnumerable<IBeforeCreate<Customer>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<Customer>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<Customer>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<Customer>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<Customer>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<Customer>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<Customer>> jsonService,
            Lazy<IAuditlogger<Customer>> auditlogger
        )
            : base(logger, currentUserIdProvider, dateTime, dbCommand, defaultValuesSetters, validators,
                beforeCreateHooks,
                afterCreateHooks, beforeUpdateHooks, afterUpdateHooks, beforeDeleteHooks, afterDeleteHooks,
                outboxEventCreator, outboxMessageCreator, jsonService, auditlogger)
        {
        }
    }
}