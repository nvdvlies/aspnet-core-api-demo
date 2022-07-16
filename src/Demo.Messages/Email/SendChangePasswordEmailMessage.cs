using System;

namespace Demo.Messages.Email;

public class
    SendChangePasswordEmailMessage : Message<SendChangePasswordEmailMessage, SendChangePasswordEmailMessageData>
{
    public static SendChangePasswordEmailMessage Create(Guid createdBy, Guid correlationId, Guid userId)
    {
        var data = new SendChangePasswordEmailMessageData { CorrelationId = correlationId, UserId = userId };
        return new SendChangePasswordEmailMessage
        {
            Queue = Queues.SendChangePasswordEmail,
            Data = data,
            Subject = $"Email/{data.UserId}",
            DataVersion = data.MessageDataVersion,
            CreatedBy = createdBy,
            CorrelationId = data.CorrelationId
        };
    }
}

public class SendChangePasswordEmailMessageData : IMessageData
{
    public Guid UserId { get; set; }
    public string MessageDataVersion => "1.0";
    public Guid CorrelationId { get; set; }
}
