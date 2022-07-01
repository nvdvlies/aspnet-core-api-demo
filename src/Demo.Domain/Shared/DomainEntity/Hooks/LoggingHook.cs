using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Demo.Domain.Shared.DomainEntity.Hooks
{
    internal class LoggingHook<T> : IBeforeCreate<T>, IBeforeUpdate<T>, IBeforeDelete<T>, IAfterCreate<T>,
        IAfterUpdate<T>, IAfterDelete<T>
        where T : Entity
    {
        private readonly ILogger<LoggingHook<T>> _logger;

        public LoggingHook(ILogger<LoggingHook<T>> logger)
        {
            _logger = logger;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<T> context, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{type}: {typeof(T).Name}");
            return Task.CompletedTask;
        }
    }
}