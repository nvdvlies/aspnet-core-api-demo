using System;

namespace Demo.Messages.User
{
    public class SyncRolesToAuth0UserMessage : Message<SyncRolesToAuth0UserMessage, SyncRolesToAuth0UserMessageData>
    {
        public static SyncRolesToAuth0UserMessage Create(string correlationId, Guid id)
        {
            var data = new SyncRolesToAuth0UserMessageData
            {
                CorrelationId = correlationId,
                Id = id
            };
            return new SyncRolesToAuth0UserMessage
            {
                Queue = Queues.SyncRolesToAuth0User,
                Data = data,
                Subject = $"User/{data.Id}",
                DataVersion = data.MessageDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class SyncRolesToAuth0UserMessageData : IMessageData
    {
        public string MessageDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
