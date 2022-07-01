using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.UserPreferences.Hooks
{
    internal class SetEntityIdBeforeCreateHook : IBeforeCreate<UserPreferences>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;

        public SetEntityIdBeforeCreateHook(ICurrentUserIdProvider currentUserIdProvider)
        {
            _currentUserIdProvider = currentUserIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<UserPreferences> context,
            CancellationToken cancellationToken)
        {
            if (context.Entity.Id == Guid.Empty)
            {
                context.Entity.Id = _currentUserIdProvider.Id;
            }

            return Task.CompletedTask;
        }
    }
}