namespace Demo.Messages
{
    public interface IMessageData
    {
        string MessageDataVersion { get; }
        string CorrelationId { get; }
    }
}
