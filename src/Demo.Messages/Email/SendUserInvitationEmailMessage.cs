using System;

namespace Demo.Messages.Email
{
    public class
        SendUserInvitationEmailMessage : Message<SendUserInvitationEmailMessage, SendUserInvitationEmailMessageData>
    {
        public static SendUserInvitationEmailMessage Create(Guid createdBy, Guid correlationId, Guid userId)
        {
            var data = new SendUserInvitationEmailMessageData { CorrelationId = correlationId, UserId = userId };
            return new SendUserInvitationEmailMessage
            {
                Queue = Queues.SendUserInvitationEmail,
                Data = data,
                Subject = $"Email/{data.UserId}",
                DataVersion = data.MessageDataVersion,
                CreatedBy = createdBy,
                CorrelationId = data.CorrelationId
            };
        }
    }

    public class SendUserInvitationEmailMessageData : IMessageData
    {
        public Guid UserId { get; set; }
        public string MessageDataVersion => "1.0";
        public Guid CorrelationId { get; set; }
    }
}
