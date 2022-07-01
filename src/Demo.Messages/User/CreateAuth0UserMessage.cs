using System;

namespace Demo.Messages.User
{
    public class CreateAuth0UserMessage : Message<CreateAuth0UserMessage, CreateAuth0UserMessageData>
    {
        public static CreateAuth0UserMessage Create(Guid createdBy, Guid correlationId, Guid id)
        {
            var data = new CreateAuth0UserMessageData { CorrelationId = correlationId, Id = id };
            return new CreateAuth0UserMessage
            {
                Queue = Queues.CreateAuth0User,
                Data = data,
                Subject = $"User/{data.Id}",
                DataVersion = data.MessageDataVersion,
                CreatedBy = createdBy,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class CreateAuth0UserMessageData : IMessageData
    {
        public Guid Id { get; set; }
        public string MessageDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}