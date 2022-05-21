using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.UserPreferences.Hooks
{
    internal class SetEntityIdBeforeCreateHook : IBeforeCreate<UserPreferences>
    {
        private readonly ICurrentUser _currentUser;

        public SetEntityIdBeforeCreateHook(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<UserPreferences> context, CancellationToken cancellationToken)
        {
            if (context.Entity.Id == Guid.Empty)
            {
                context.Entity.Id = _currentUser.Id;
            }

            return Task.CompletedTask;
        }
    }
}