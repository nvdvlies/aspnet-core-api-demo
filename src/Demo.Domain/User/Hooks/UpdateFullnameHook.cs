using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Domain.User.Hooks
{
    internal class UpdateFullnameHook : IBeforeCreate<User>, IBeforeUpdate<User>
    {
        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken)
        {
            context.Entity.Fullname =
                $"{context.Entity.GivenName} {context.Entity.MiddleName} {context.Entity.FamilyName}"
                    .Replace("  ", " ")
                    .Trim();
            return Task.CompletedTask;
        }
    }
}