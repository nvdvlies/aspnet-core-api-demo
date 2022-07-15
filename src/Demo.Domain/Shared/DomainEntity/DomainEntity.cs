using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Exceptions;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
using Demo.Messages;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Shared.DomainEntity
{
    internal abstract class DomainEntity<T> : IDomainEntity<T> where T : IEntity
    {
        private readonly Lazy<IEnumerable<IAfterCreate<T>>> _afterCreateHooks;
        private readonly Lazy<IEnumerable<IAfterDelete<T>>> _afterDeleteHooks;
        private readonly Lazy<IEnumerable<IAfterUpdate<T>>> _afterUpdateHooks;
        private readonly Lazy<IAuditlogger<T>> _auditlogger;
        private readonly Lazy<IEnumerable<IBeforeCreate<T>>> _beforeCreateHooks;
        private readonly Lazy<IEnumerable<IBeforeDelete<T>>> _beforeDeleteHooks;
        private readonly Lazy<IEnumerable<IBeforeUpdate<T>>> _beforeUpdateHooks;

        private readonly Lazy<IEnumerable<IDefaultValuesSetter<T>>> _defaultValuesSetters;

        private readonly DomainEntityOptions _options;
        private readonly Lazy<IEnumerable<IValidator<T>>> _validators;
        protected readonly IDomainEntityContext<T> Context;
        protected readonly ICurrentUserIdProvider CurrentUserIdProvider;
        protected readonly IDateTime DateTime;
        protected readonly IDbCommand<T> DbCommand;
        protected readonly ILogger Logger;

        protected DomainEntity(
            ILogger logger,
            ICurrentUserIdProvider currentUserIdProvider,
            IDateTime dateTime,
            IDbCommand<T> dbCommand,
            Lazy<IEnumerable<IDefaultValuesSetter<T>>> defaultValuesSetters,
            Lazy<IEnumerable<IValidator<T>>> validators,
            Lazy<IEnumerable<IBeforeCreate<T>>> beforeCreateHooks,
            Lazy<IEnumerable<IAfterCreate<T>>> afterCreateHooks,
            Lazy<IEnumerable<IBeforeUpdate<T>>> beforeUpdateHooks,
            Lazy<IEnumerable<IAfterUpdate<T>>> afterUpdateHooks,
            Lazy<IEnumerable<IBeforeDelete<T>>> beforeDeleteHooks,
            Lazy<IEnumerable<IAfterDelete<T>>> afterDeleteHooks,
            Lazy<IOutboxEventCreator> outboxEventCreator,
            Lazy<IOutboxMessageCreator> outboxMessageCreator,
            Lazy<IJsonService<T>> jsonService,
            Lazy<IAuditlogger<T>> auditlogger
        )
        {
            Context = new DomainEntityContext<T>(logger, outboxEventCreator, outboxMessageCreator, jsonService);
            Logger = logger;
            CurrentUserIdProvider = currentUserIdProvider;
            DateTime = dateTime;
            DbCommand = dbCommand;

            _defaultValuesSetters = defaultValuesSetters;
            _validators = validators;
            _beforeCreateHooks = beforeCreateHooks;
            _afterCreateHooks = afterCreateHooks;
            _beforeUpdateHooks = beforeUpdateHooks;
            _afterUpdateHooks = afterUpdateHooks;
            _beforeDeleteHooks = beforeDeleteHooks;
            _afterDeleteHooks = afterDeleteHooks;
            _auditlogger = auditlogger;

            _options = new DomainEntityOptions();
        }

        protected IReadonlyDomainEntityOptions Options => _options;

        protected virtual Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes => null;

        public T Entity => Context.Entity;

        public Guid EntityId => Context.Entity?.Id ?? Guid.Empty;

        public IDomainEntityState State => Context.State;

        public virtual async Task NewAsync(CancellationToken cancellationToken = default)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(NewAsync));
            try
            {
                var entity = Activator.CreateInstance<T>();
                foreach (var defaultValuesSetter in _defaultValuesSetters.Value.OrderBy(x => x.Order))
                {
                    await defaultValuesSetter.SetDefaultValuesAsync(entity, Context.State, cancellationToken);
                }

                Context.Entity = entity;
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        public virtual IDomainEntity<T> WithOptions(Action<IDomainEntityOptions> action)
        {
            action(_options);
            DbCommand.WithOptions(x => { x.AsNoTracking = Options.AsNoTracking; });
            return this;
        }

        public virtual async Task GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(GetAsync));
            try
            {
                Context.Entity = await DbCommand.GetAsync(id, Includes, cancellationToken);

                if (Context.Entity == null
                    || (Context.Entity is ISoftDeleteEntity && !Options.IncludeDeleted &&
                        ((ISoftDeleteEntity)Context.Entity).Deleted))
                {
                    throw new DomainEntityNotFoundException($"Entity with id '{id}' not found");
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        public virtual void With(Action<T> action)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity),
                $"Call {nameof(NewAsync)} or {nameof(GetAsync)} first to instantiate entity");

            action(Context.Entity);
        }

        public virtual async Task CreateAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity),
                $"Call {nameof(NewAsync)} first to instantiate entity");

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(CreateAsync));
            try
            {
                Context.EditMode = EditMode.Create;

                await ExecuteBeforeCreateHooks(cancellationToken);

                await ValidateAsync(cancellationToken);

                if (Context.Entity is IAuditableEntity auditableEntity)
                {
                    auditableEntity.SetCreatedByAndCreatedOn(CurrentUserIdProvider.Id, DateTime.UtcNow);
                }

                await DbCommand.InsertAsync(Context.Entity, cancellationToken);

                await ExecuteAfterCreateHooks(cancellationToken);
            }
            finally
            {
                stopwatch.Stop();
                Context.PerformanceMeasurements.Flush();
            }
        }

        public virtual async Task UpdateAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity),
                $"Call {nameof(GetAsync)} first to instantiate entity");
            Guard.NotTrue(Options.AsNoTracking, nameof(Options.AsNoTracking),
                $"Cannot update entity with option '{nameof(Options.AsNoTracking)}' set to 'true'");

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(UpdateAsync));
            try
            {
                Context.EditMode = EditMode.Update;

                await ExecuteBeforeUpdateHooks(cancellationToken);

                await ValidateAsync(cancellationToken);

                if (Context.Entity is IAuditableEntity auditableEntity)
                {
                    auditableEntity.SetLastModifiedByAndLastModifiedOn(CurrentUserIdProvider.Id, DateTime.UtcNow);
                }

                await DbCommand.UpdateAsync(Context.Entity, cancellationToken);

                await ExecuteAfterUpdateHooks(cancellationToken);

                await CreateAuditLogAsync(cancellationToken);
            }
            finally
            {
                stopwatch.Stop();
                Context.PerformanceMeasurements.Flush();
            }
        }

        public virtual async Task UpsertAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity),
                $"Call {nameof(NewAsync)} or {nameof(GetAsync)} first to instantiate entity");

            if (EntityId == Guid.Empty)
            {
                await CreateAsync(cancellationToken);
            }
            else
            {
                await UpdateAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            Guard.NotNull(Context.Entity, nameof(Context.Entity),
                $"Call {nameof(GetAsync)} first to instantiate entity");
            Guard.NotTrue(Options.AsNoTracking, nameof(Options.AsNoTracking),
                $"Cannot delete entity with option '{nameof(Options.AsNoTracking)}' set to 'true'");

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(DeleteAsync));
            try
            {
                Context.EditMode = EditMode.Delete;

                await ExecuteBeforeDeleteHooks(cancellationToken);

                await ValidateAsync(cancellationToken);

                if (Context.Entity is ISoftDeleteEntity softDeleteEntity && !Options.DisableSoftDelete)
                {
                    softDeleteEntity.MarkAsDeleted(CurrentUserIdProvider.Id, DateTime.UtcNow);

                    await DbCommand.UpdateAsync(Context.Entity, cancellationToken);
                }
                else
                {
                    await DbCommand.DeleteAsync(Context.Entity, cancellationToken);
                }

                await ExecuteAfterDeleteHooks(cancellationToken);
            }
            finally
            {
                stopwatch.Stop();
                Context.PerformanceMeasurements.Flush();
            }
        }

        public Task AddEventAsync(IEvent @event, CancellationToken cancellationToken)
        {
            return Context.AddEventAsync(@event, cancellationToken);
        }

        public Task AddMessageAsync(IMessage message, CancellationToken cancellationToken)
        {
            return Context.AddMessageAsync(message, cancellationToken);
        }

        private async Task ValidateAsync(CancellationToken cancellationToken = default)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ValidateAsync));
            try
            {
                var validationTasks = _validators.Value
                    .Select(v => v.ValidateAsync(Context, cancellationToken));

                var validationMessages = (await Task.WhenAll(validationTasks))
                    .Where(x => x != null)
                    .SelectMany(x => x)
                    .ToList();

                if (validationMessages.Count != 0)
                {
                    throw new DomainValidationException(validationMessages);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ExecuteBeforeCreateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteBeforeCreateHooks));
            try
            {
                foreach (var beforeCreateHook in _beforeCreateHooks.Value.OrderBy(x => x.Order))
                {
                    await beforeCreateHook.ExecuteAsync(HookType.BeforeCreate, Context, cancellationToken);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ExecuteAfterCreateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteAfterCreateHooks));
            try
            {
                foreach (var afterCreateHook in _afterCreateHooks.Value.OrderBy(x => x.Order))
                {
                    await afterCreateHook.ExecuteAsync(HookType.AfterCreate, Context, cancellationToken);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ExecuteBeforeUpdateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteBeforeUpdateHooks));
            try
            {
                foreach (var beforeUpdateHook in _beforeUpdateHooks.Value.OrderBy(x => x.Order))
                {
                    await beforeUpdateHook.ExecuteAsync(HookType.BeforeUpdate, Context, cancellationToken);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ExecuteAfterUpdateHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteAfterUpdateHooks));
            try
            {
                foreach (var afterUpdateHook in _afterUpdateHooks.Value.OrderBy(x => x.Order))
                {
                    await afterUpdateHook.ExecuteAsync(HookType.AfterUpdate, Context, cancellationToken);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ExecuteBeforeDeleteHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteBeforeDeleteHooks));
            try
            {
                foreach (var beforeDeleteHook in _beforeDeleteHooks.Value.OrderBy(x => x.Order))
                {
                    await beforeDeleteHook.ExecuteAsync(HookType.BeforeDelete, Context, cancellationToken);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ExecuteAfterDeleteHooks(CancellationToken cancellationToken)
        {
            var stopwatch = Context.PerformanceMeasurements.Start(nameof(ExecuteAfterDeleteHooks));
            try
            {
                foreach (var afterDeleteHook in _afterDeleteHooks.Value.OrderBy(x => x.Order))
                {
                    await afterDeleteHook.ExecuteAsync(HookType.AfterDelete, Context, cancellationToken);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task CreateAuditLogAsync(CancellationToken cancellationToken)
        {
            if (_auditlogger == null || _auditlogger.Value == null)
            {
                return;
            }

            var stopwatch = Context.PerformanceMeasurements.Start(nameof(CreateAuditLogAsync));
            try
            {
                await _auditlogger.Value.CreateAuditLogAsync(Context.Entity, Context.Pristine, cancellationToken);
            }
            finally
            {
                stopwatch.Stop();
            }
        }
    }
}