using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings.DomainEntity.Events;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.DomainEntity.Hooks
{
    internal class ApplicationSettingsUpdatedDomainEventHook : IAfterCreate<ApplicationSettings>, IAfterUpdate<ApplicationSettings>
    {
        private readonly ICurrentUser _currentUser;

        public ApplicationSettingsUpdatedDomainEventHook(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<ApplicationSettings> context, CancellationToken cancellationToken)
        {
            context.PublishDomainEventAfterCommit(new ApplicationSettingsUpdatedDomainEvent(context.Entity.Id, _currentUser.Id));
            return Task.CompletedTask;
        }
    }
}
