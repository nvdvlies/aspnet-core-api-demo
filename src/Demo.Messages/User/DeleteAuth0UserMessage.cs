using System;

namespace Demo.Messages.User
{
    public class DeleteAuth0UserMessage : Message<DeleteAuth0UserMessage, DeleteAuth0UserMessageData>
    {
        public static DeleteAuth0UserMessage Create(Guid correlationId, Guid id)
        {
            var data = new DeleteAuth0UserMessageData { CorrelationId = correlationId, Id = id };
            return new DeleteAuth0UserMessage
            {
                Queue = Queues.DeleteAuth0User,
                Data = data,
                Subject = $"User/{data.Id}",
                DataVersion = data.MessageDataVersion,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class DeleteAuth0UserMessageData : IMessageData
    {
        public Guid Id { get; set; }
        public string MessageDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}
