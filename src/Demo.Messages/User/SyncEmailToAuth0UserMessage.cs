using System;

namespace Demo.Messages.User
{
    public class SyncEmailToAuth0UserMessage : Message<SyncEmailToAuth0UserMessage, SyncEmailToAuth0UserMessageData>
    {
        public static SyncEmailToAuth0UserMessage Create(string correlationId, Guid id)
        {
            var data = new SyncEmailToAuth0UserMessageData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new SyncEmailToAuth0UserMessage
            {
                Queue = Queues.SyncEmailToAuth0User,
                Data = data,
                Subject = $"User/{data.Id}",
                DataVersion = data.MessageDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class SyncEmailToAuth0UserMessageData : IMessageData
    {
        public string MessageDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
