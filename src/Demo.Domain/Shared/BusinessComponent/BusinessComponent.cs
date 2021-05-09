using Demo.Common.Interfaces;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.BusinessComponent
{
    internal abstract class BusinessComponent<T> : IBusinessComponent<T> where T : IEntity
    {
        protected readonly IBusinessComponentContext<T> Context;
        protected BusinessComponentOptions Options { get; private set; }
        protected readonly ICurrentUser CurrentUser;
        protected readonly IDateTime DateTime;
        protected readonly IDbCommand<T> DbCommand;
        protected readonly ILogger Logger;

        private readonly IEnumerable<IDefaultValuesSetter<T>> _defaultValuesSetters;
        private readonly IEnumerable<IValidator<T>> _validators;
        private readonly IEnumerable<IBeforeCreate<T>> _beforeCreateHooks;
        private readonly IEnumerable<IAfterCreate<T>> _afterCreateHooks;
        private readonly IEnumerable<IBeforeUpdate<T>> _beforeUpdateHooks;
        private readonly IEnumerable<IAfterUpdate<T>> _afterUpdateHooks;
        private readonly IEnumerable<IBeforeDelete<T>> _beforeDeleteHooks;
        private readonly IEnumerable<IAfterDelete<T>> _afterDeleteHooks;
        private readonly IAuditlog<T> _auditlog;

        public BusinessComponent(
            ILogger logger,
            ICurrentUser currentUser,
            IDateTime dateTime,
            IDbCommand<T> dbCommand,
            IEnumerable<IDefaultValuesSetter<T>> defaultValuesSetters,
            IEnumerable<IValidator<T>> validators,
            IEnumerable<IBeforeCreate<T>> beforeCreateHooks,
            IEnumerable<IAfterCreate<T>> afterCreateHooks,
            IEnumerable<IBeforeUpdate<T>> beforeUpdateHooks,
            IEnumerable<IAfterUpdate<T>> afterUpdateHooks,
            IEnumerable<IBeforeDelete<T>> beforeDeleteHooks,
            IEnumerable<IAfterDelete<T>> afterDeleteHooks,
            IPublishDomainEventAfterCommitQueue publishDomainEventAfterCommitQueue,
            IJsonService<T> jsonService,
            IAuditlog<T> auditlog
        )
        {
            Context = new BusinessComponentContext<T>(logger, publishDomainEventAfterCommitQueue, jsonService);
            Options = new BusinessComponentOptions();
            Logger = logger;
            CurrentUser = currentUser;
            DateTime = dateTime;
            DbCommand = dbCommand;

            _defaultValuesSetters = defaultValuesSetters;
            _validators = validators;
            _beforeCreateHooks = beforeCreateHooks.OrderBy(x => x.Order);
            _afterCreateHooks = afterCreateHooks.OrderBy(x => x.Order);
            _beforeUpdateHooks = beforeUpdateHooks.OrderBy(x => x.Order);
            _afterUpdateHooks = afterUpdateHooks.OrderBy(x => x.Order);
            _beforeDeleteHooks = beforeDeleteHooks.OrderBy(x => x.Order);
            _afterDeleteHooks = afterDeleteHooks.OrderBy(x => x.Order);
            _auditlog = auditlog;
        }

        public T Entity => Context.Entity;

        public Guid EntityId => Context.Entity?.Id ?? default;

        internal virtual Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes => null;

        public IBusinessComponentState State => Context.State;

        public async virtual Task NewAsync(CancellationToken cancellationToken = default)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(NewAsync));

            var entity = Activator.CreateInstance<T>();
            foreach (var defaultValuesSetter in _defaultValuesSetters)
            {
                await defaultValuesSetter.SetDefaultValuesAsync(entity, Context.State, cancellationToken);
            }
            Context.Entity = entity;

            stopwatch.Stop();
        }

        public virtual IBusinessComponent<T> WithOptions(Action<BusinessComponentOptions> action)
        {
            action(Options);
            DbCommand.WithOptions(x => {
                x.AsNoTracking = Options.AsNoTracking;
            });
            return this;
        }

        public async virtual Task GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(GetAsync));

            Context.Entity = await DbCommand.GetAsync(id, Includes, cancellationToken);

            if (Context.Entity == null
                || (Context.Entity is ISoftDeleteEntity softDeleteEntity && !Options.IncludeDeleted && ((ISoftDeleteEntity)Context.Entity).Deleted))
            {
                throw new DomainEntityNotFoundException($"Entity with id '{id}' not found");
            }

            stopwatch.Stop();
        }

        public virtual void With(Action<T> action)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity), "Call NewAsync or GetAsync first to instantiate entity");

            action(Context.Entity);
        }

        private async Task ValidateAsync(EditMode editMode, CancellationToken cancellationToken = default)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ValidateAsync));

            var validationTasks = _validators
                .Select(async v => await v.ValidateAsync(Context, cancellationToken));

            var validationMessages = (await Task.WhenAll(validationTasks))
                .Where(x => x != null)
                .SelectMany(x => x)
                .ToList();

            stopwatch.Stop();

            if (validationMessages.Count != 0)
            {
                throw new DomainValidationException(validationMessages);
            }
        }

        public async virtual Task CreateAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity), "Call NewAsync first to instantiate entity");

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(CreateAsync));

            Context.EditMode = EditMode.Create;

            await ExecuteBeforeCreateHooks(cancellationToken);

            await ValidateAsync(Context.EditMode, cancellationToken);

            if (Context.Entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.SetCreatedByAndCreatedOn(CurrentUser.Id, DateTime.UtcNow);
            }

            await DbCommand.InsertAsync(Context.Entity, cancellationToken);

            await ExecuteAfterCreateHooks(cancellationToken);

            stopwatch.Stop();

            Context.PerformanceMeasurements.Flush();
        }

        private async Task ExecuteBeforeCreateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteBeforeCreateHooks));
            foreach (var beforeCreateHook in _beforeCreateHooks)
            {
                await beforeCreateHook.ExecuteAsync(HookType.BeforeCreate, Context, cancellationToken);
            }
            stopwatch.Stop();
        }

        private async Task ExecuteAfterCreateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteAfterCreateHooks));
            foreach (var afterCreateHook in _afterCreateHooks)
            {
                await afterCreateHook.ExecuteAsync(HookType.AfterCreate, Context, cancellationToken);
            }
            stopwatch.Stop();
        }

        public async virtual Task UpdateAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity), "Call GetAsync first to instantiate entity");
            Guard.NotTrue(Options.AsNoTracking, nameof(Options.AsNoTracking), "Cannot update entity with option 'AsNoTracking' set to 'true'");

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(UpdateAsync));

            Context.EditMode = EditMode.Update;

            await ExecuteBeforeUpdateHooks(cancellationToken);

            await ValidateAsync(Context.EditMode, cancellationToken);

            if (Context.Entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.SetLastModifiedByAndLastModifiedOn(CurrentUser.Id, DateTime.UtcNow);
            }

            await DbCommand.UpdateAsync(Context.Entity, cancellationToken);

            await ExecuteAfterUpdateHooks(cancellationToken);

            await CreateAuditLogAsync(cancellationToken);

            stopwatch.Stop();

            Context.PerformanceMeasurements.Flush();
        }

        private async Task ExecuteBeforeUpdateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteBeforeUpdateHooks));
            foreach (var beforeUpdateHook in _beforeUpdateHooks)
            {
                await beforeUpdateHook.ExecuteAsync(HookType.BeforeUpdate, Context, cancellationToken);
            }
            stopwatch.Stop();
        }

        private async Task ExecuteAfterUpdateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteAfterUpdateHooks));
            foreach (var afterUpdateHook in _afterUpdateHooks)
            {
                await afterUpdateHook.ExecuteAsync(HookType.AfterUpdate, Context, cancellationToken);
            }
            stopwatch.Stop();
        }

        public async virtual Task UpsertAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity), "Call NewAsync or GetAsync first to instantiate entity");

            if (EntityId == default)
            {
                await CreateAsync(cancellationToken);
            } 
            else
            {
                await UpdateAsync(cancellationToken);
            }
        }

        public async virtual Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity), "Call GetAsync first to instantiate entity");
            Guard.NotTrue(Options.AsNoTracking, nameof(Options.AsNoTracking), "Cannot delete entity with option 'AsNoTracking' set to 'true'");

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(DeleteAsync));

            Context.EditMode = EditMode.Delete;

            await ExecuteBeforeDeleteHooks(cancellationToken);

            await ValidateAsync(Context.EditMode, cancellationToken);

            if (Context.Entity is ISoftDeleteEntity softDeleteEntity && !Options.DisableSoftDelete)
            {
                softDeleteEntity.MarkAsDeleted(CurrentUser.Id, DateTime.UtcNow);

                await DbCommand.UpdateAsync(Context.Entity, cancellationToken);
            }
            else
            {
                await DbCommand.DeleteAsync(Context.Entity, cancellationToken);
            }

            await ExecuteAfterDeleteHooks(cancellationToken);

            stopwatch.Stop();

            Context.PerformanceMeasurements.Flush();
        }

        private async Task ExecuteBeforeDeleteHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteBeforeDeleteHooks));
            foreach (var beforeDeleteHook in _beforeDeleteHooks)
            {
                await beforeDeleteHook.ExecuteAsync(HookType.BeforeDelete, Context, cancellationToken);
            }
            stopwatch.Stop();
        }

        private async Task ExecuteAfterDeleteHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteAfterDeleteHooks));
            foreach (var afterDeleteHook in _afterDeleteHooks)
            {
                await afterDeleteHook.ExecuteAsync(HookType.AfterDelete, Context, cancellationToken);
            }
            stopwatch.Stop();
        }

        public virtual void PublishDomainEventAfterCommit(IDomainEvent domainEvent)
        {
            Context.PublishDomainEventAfterCommit(domainEvent);
        }

        private async Task CreateAuditLogAsync(CancellationToken cancellationToken)
        {
            if (_auditlog == null)
            {
                return;
            }

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(CreateAuditLogAsync));
            await _auditlog.CreateAuditLogAsync(Context.Entity, Context.Pristine, cancellationToken);
            stopwatch.Stop();
        }
    }
}
