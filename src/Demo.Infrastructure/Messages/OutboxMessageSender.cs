using Demo.Application.Shared.Interfaces;
using Demo.Domain.OutboxMessage.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Messages
{
    internal class OutboxMessageSender : IOutboxMessageSender
    {
        private readonly ILogger<OutboxMessageCreator> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEntityFactory _domainEntityFactory;
        private readonly IMessageSender _messageSender;

        public OutboxMessageSender(
            ILogger<OutboxMessageCreator> logger,
            IUnitOfWork unitOfWork,
            IDomainEntityFactory domainEntityFactory,
            IMessageSender messageSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _domainEntityFactory = domainEntityFactory;
            _messageSender = messageSender;
        }

        public async Task SendAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending OutboxMessage entity with id '{id}'", id);

            var outboxMessageDomainEntity = _domainEntityFactory.CreateInstance<IOutboxMessageDomainEntity>();
            await outboxMessageDomainEntity.GetAsync(id, cancellationToken);

            _logger.LogInformation("Sending message of type '{type}'", outboxMessageDomainEntity.Type);

            bool isSent = false;
            try
            {
                var message = outboxMessageDomainEntity.GetMessage();
                await _messageSender.SendAsync(message, cancellationToken);
                _logger.LogInformation("Sent message of type '{type}'", outboxMessageDomainEntity.Type);
                isSent = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message of type '{type}'  (ID: {id})", outboxMessageDomainEntity.Type, outboxMessageDomainEntity.EntityId);
                await UnLockAsync(outboxMessageDomainEntity, cancellationToken);
            }

            if (isSent)
            {
                try
                {
                    await UnlockAndMarkAsSentAsync(outboxMessageDomainEntity, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Failed to mark sent message of type '{type}' as sent (ID: {id})", outboxMessageDomainEntity.Type, outboxMessageDomainEntity.EntityId);
                }
            }
        }

        private async Task UnLockAsync(IOutboxMessageDomainEntity outboxMessageDomainEntity, CancellationToken cancellationToken)
        {
            outboxMessageDomainEntity.Unlock();
            await outboxMessageDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task UnlockAndMarkAsSentAsync(IOutboxMessageDomainEntity outboxMessageDomainEntity, CancellationToken cancellationToken)
        {
            outboxMessageDomainEntity.Unlock();
            outboxMessageDomainEntity.MarkAsSent();
            await outboxMessageDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
