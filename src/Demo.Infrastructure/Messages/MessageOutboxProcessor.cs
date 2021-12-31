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

                _logger.LogInformation("Sending message of type '{type}'", messageOutbox.Type);

                bool isSent = false;
                try
                {
                    await _messageOutboxProcessor.SendAsync(messageOutbox.Message, cancellationToken);
                    
                    _logger.LogInformation("Sent message of type '{type}'", messageOutbox.Type);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send message ({type})", messageOutbox.Type);
                    await UnLockAsync(messageOutbox, cancellationToken);
                }

                if (isSent)
                {
                    try
                    {
                        await MarkAsSent(messageOutbox, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "Failed to mark sent message as sent (Type: {type}, ID: {id})", messageOutbox.Type, messageOutbox.Id);
                    }
                }
            }
        }

        private async Task UnLockAsync(MessageOutbox messageOutbox, CancellationToken cancellationToken)
        {
            messageOutbox.Unlock();
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
