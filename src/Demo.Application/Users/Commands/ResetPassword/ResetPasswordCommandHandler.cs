using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages.Email;

namespace Demo.Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IOutboxMessageCreator _outboxMessageCreator;

        public ResetPasswordCommandHandler(
            ICurrentUserIdProvider currentUserIdProvider,
            ICorrelationIdProvider correlationIdProvider,
            IOutboxMessageCreator outboxMessageCreator
        )
        {
            _currentUserIdProvider = currentUserIdProvider;
            _correlationIdProvider = correlationIdProvider;
            _outboxMessageCreator = outboxMessageCreator;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _outboxMessageCreator.CreateAsync(
                SendChangePasswordEmailMessage.Create(_currentUserIdProvider.Id, _correlationIdProvider.Id, request.Id),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}