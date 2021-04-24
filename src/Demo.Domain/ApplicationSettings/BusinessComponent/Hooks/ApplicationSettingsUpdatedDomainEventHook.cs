using Demo.Common.Interfaces;
using Demo.Domain.ApplicationSettings.BusinessComponent.Events;
using Demo.Domain.Shared.BusinessComponent;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationSettings.BusinessComponent.Hooks
{
    internal class ApplicationSettingsUpdatedDomainEventHook : IAfterCreate<ApplicationSettings>, IAfterUpdate<ApplicationSettings>
    {
        private readonly ICurrentUser _currentUser;

        public ApplicationSettingsUpdatedDomainEventHook(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Task ExecuteAsync(HookType type, IBusinessComponentContext<ApplicationSettings> context, CancellationToken cancellationToken)
        {
            context.PublishDomainEventAfterCommit(new ApplicationSettingsUpdatedDomainEvent(context.Entity.Id, _currentUser.Id));
            return Task.CompletedTask;
        }
    }
}
