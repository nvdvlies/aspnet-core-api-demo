using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages.User;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.User.Hooks
{
    internal class CreateAuth0UserHook : IAfterCreate<User>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public CreateAuth0UserHook(
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken)
        {
            context.AddMessageAsync(CreateAuth0UserMessage.Create(_correlationIdProvider.Id, context.Entity.Id), cancellationToken);
            return Task.CompletedTask;
        }
    }
}