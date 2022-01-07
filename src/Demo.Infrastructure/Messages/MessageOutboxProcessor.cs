using Demo.Application.Shared.Interfaces;
using Demo.Domain.MessageOutbox.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Messages
{
    internal class MessageOutboxProcessor : IMessageOutboxProcessor
    {
        private readonly ILogger<MessageOutboxProcessor> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEntityFactory _domainEntityFactory;
        private readonly IMessageSender _messageOutboxProcessor;
        private Queue<IMessageOutboxDomainEntity> _queue;

        public MessageOutboxProcessor(
            ILogger<MessageOutboxProcessor> logger,
            IUnitOfWork unitOfWork,
            IDomainEntityFactory domainEntityFactory,
            IMessageSender messageOutboxProcessor)
        {
            _queue = new Queue<IMessageOutboxDomainEntity>();
            _logger = logger;
            _unitOfWork = unitOfWork;
            _domainEntityFactory = domainEntityFactory;
            _messageOutboxProcessor = messageOutboxProcessor;
        }

        public async Task AddToOutboxAsync(IMessage message, CancellationToken cancellationToken = default)
        {
            var messageOutboxDomainEntity = _domainEntityFactory.CreateInstance<IMessageOutboxDomainEntity>();
            await messageOutboxDomainEntity.NewAsync(cancellationToken);
            messageOutboxDomainEntity.SetMessage(message);
            messageOutboxDomainEntity.Lock();
            await messageOutboxDomainEntity.CreateAsync(cancellationToken);

            _queue.Enqueue(messageOutboxDomainEntity);
        }

        public async Task SendAllAsync(CancellationToken cancellationToken = default)
        {
            while (_queue.Count > 0)
            {
                var messageOutboxDomainEntity = _queue.Dequeue();

                _logger.LogInformation("Sending message of type '{type}'", messageOutboxDomainEntity.Type);

                bool isSent = false;
                try
                {
                    var message = messageOutboxDomainEntity.GetMessage();
                    await _messageOutboxProcessor.SendAsync(message, cancellationToken);
                    _logger.LogInformation("Sent message of type '{type}'", messageOutboxDomainEntity.Type);
                    isSent = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send message of type '{type}'  (ID: {id})", messageOutboxDomainEntity.Type, messageOutboxDomainEntity.EntityId);
                    await UnLockAsync(messageOutboxDomainEntity, cancellationToken);
                }

                if (isSent)
                {
                    try
                    {
                        await UnlockAndMarkAsSentAsync(messageOutboxDomainEntity, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "Failed to mark sent message of type '{type}' as sent (ID: {id})", messageOutboxDomainEntity.Type, messageOutboxDomainEntity.EntityId);
                    }
                }
            }
        }

        private async Task UnLockAsync(IMessageOutboxDomainEntity messageOutboxDomainEntity, CancellationToken cancellationToken)
        {
            messageOutboxDomainEntity.Unlock();
            await messageOutboxDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task UnlockAndMarkAsSentAsync(IMessageOutboxDomainEntity messageOutboxDomainEntity, CancellationToken cancellationToken)
        {
            messageOutboxDomainEntity.Unlock();
            messageOutboxDomainEntity.MarkAsSent();
            await messageOutboxDomainEntity.UpdateAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
