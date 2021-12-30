using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Events;
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
        private readonly IMessageSender _messageOutboxProcessor;
        private readonly IDbCommand<MessageOutbox> _dbCommand;
        private Queue<MessageOutbox> _queue;

        public MessageOutboxProcessor(
            ILogger<MessageOutboxProcessor> logger,
            IUnitOfWork unitOfWork,
            IMessageSender messageOutboxProcessor,
            IDbCommand<MessageOutbox> dbCommand)
        {
            _queue = new Queue<MessageOutbox>();
            _logger = logger;
            _unitOfWork = unitOfWork;
            _messageOutboxProcessor = messageOutboxProcessor;
            _dbCommand = dbCommand;
        }

        public int Count => _queue.Count;

        public async Task AddToOutboxAsync(Message message, CancellationToken cancellationToken = default)
        {
            var messageOutbox = new MessageOutbox
            {
                Type = message.Type,
                Message = message
            };
            messageOutbox.Lock();
            await _dbCommand.InsertAsync(messageOutbox, cancellationToken);
            _queue.Enqueue(messageOutbox);
        }

        public async Task SendAllAsync(CancellationToken cancellationToken = default)
        {
            while (_queue.Count > 0)
            {
                var messageOutbox = _queue.Dequeue();
                string lockToken = null;
                try
                {
                    _logger.LogInformation($"Sending '{messageOutbox.Type}' message");

                    lockToken = await LockAsync(messageOutbox, cancellationToken);
                    await _messageOutboxProcessor.SendAsync(messageOutbox.Message, cancellationToken);
                    await MarkAsSent(messageOutbox, cancellationToken);

                    _logger.LogInformation($"Sent '{messageOutbox.Type}' message");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send message");
                    await UnLockAsync(messageOutbox, lockToken, cancellationToken);
                }
            }
        }

        private async Task<string> LockAsync(MessageOutbox messageOutbox, CancellationToken cancellationToken)
        {
            var lockToken = messageOutbox.Lock();
            await _dbCommand.UpdateAsync(messageOutbox, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return lockToken;
        }

        private async Task UnLockAsync(MessageOutbox messageOutbox, string lockToken, CancellationToken cancellationToken)
        {
            messageOutbox.Unlock(lockToken);
            await _dbCommand.UpdateAsync(messageOutbox, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task MarkAsSent(MessageOutbox messageOutbox, CancellationToken cancellationToken)
        {
            messageOutbox.MarkAsSent();
            await _dbCommand.UpdateAsync(messageOutbox, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
