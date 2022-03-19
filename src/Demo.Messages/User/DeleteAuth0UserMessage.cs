using System;

namespace Demo.Messages.User
{
    public class DeleteAuth0UserMessage : Message<DeleteAuth0UserMessage, DeleteAuth0UserMessageData>
    {
        public static DeleteAuth0UserMessage Create(string correlationId, Guid id)
        {
            var data = new DeleteAuth0UserMessageData
            {
                CorrelationId = correlationId,
                Id = id
            };
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
        public string MessageDataVersion => "1.0";
        public string CorrelationId { get; set; }

        public Guid Id { get; set; }
    }
}
