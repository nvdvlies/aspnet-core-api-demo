using System;

namespace Demo.Messages.User
{
    public class SyncNameToAuth0UserMessage : Message<SyncNameToAuth0UserMessage, SyncNameToAuth0UserMessageData>
    {
        public static SyncNameToAuth0UserMessage Create(string correlationId, Guid id)
        {
            var data = new SyncNameToAuth0UserMessageData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new SyncNameToAuth0UserMessage
            {
                Queue = Queues.SyncNameToAuth0User,
                Data = data,
                Subject = $"User/{data.Id}",
                DataVersion = data.MessageDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class SyncNameToAuth0UserMessageData : IMessageData
    {
        public string MessageDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
