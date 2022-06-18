using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages.User;

namespace Demo.Domain.User.Hooks
{
    internal class DeleteAuth0UserHook : IAfterDelete<User>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public DeleteAuth0UserHook(
            ICorrelationIdProvider correlationIdProvider
        )
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public Task ExecuteAsync(HookType type, IDomainEntityContext<User> context, CancellationToken cancellationToken)
        {
            context.AddMessageAsync(DeleteAuth0UserMessage.Create(_correlationIdProvider.Id, context.Entity.Id),
                cancellationToken);
            return Task.CompletedTask;
        }
    }
}
