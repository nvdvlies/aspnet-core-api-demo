using System;

namespace Demo.Messages.User
{
    public class SyncAuth0UserMessage : Message<SyncAuth0UserMessage, SyncAuth0UserMessageData>
    {
        public static SyncAuth0UserMessage Create(Guid correlationId, Guid id, bool emailChanged)
        {
            var data = new SyncAuth0UserMessageData
            {
                CorrelationId = correlationId,
                Id = id,
                EmailChanged = emailChanged
            };
            return new SyncAuth0UserMessage
            {
                Queue = Queues.SyncAuth0User,
                Data = data,
                Subject = $"User/{data.Id}",
                DataVersion = data.MessageDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class SyncAuth0UserMessageData : IMessageData
    {
        public Guid Id { get; set; }
        public bool EmailChanged { get; set; }
        public string MessageDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}