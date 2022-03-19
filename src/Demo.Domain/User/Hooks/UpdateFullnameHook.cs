using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.User.Hooks
{
    internal class UpdateFullnameHook : IAfterCreate<User>, IAfterUpdate<User>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken)
        {
            context.Entity.Fullname = $"{context.Entity.GivenName} {context.Entity.MiddleName} {context.Entity.FamilyName}"
                .Replace("  ", " ")
                .Trim();
            return Task.CompletedTask;
        }
    }
}