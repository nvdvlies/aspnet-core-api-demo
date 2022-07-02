using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages.Email;

namespace Demo.Application.CurrentUser.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IOutboxMessageCreator _outboxMessageCreator;

        public ChangePasswordCommandHandler(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider,
            IOutboxMessageCreator outboxMessageCreator
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
            _outboxMessageCreator = outboxMessageCreator;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _outboxMessageCreator.CreateAsync(
                SendChangePasswordEmailMessage.Create(_currentUserIdProvider.Id, _correlationIdProvider.Id, _currentUserIdProvider.Id),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}