using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.UserPreferences.Hooks
{
    public class IdDefaultValueSetter : IDefaultValuesSetter<UserPreferences>
    {
        private readonly ICurrentUser _currentUser;

        public IdDefaultValueSetter(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public Task SetDefaultValuesAsync(UserPreferences entity, IDomainEntityState state,
            CancellationToken cancellationToken = default)
        {
            entity.Id = _currentUser.Id;
            
            return Task.CompletedTask;
        }
    }
}