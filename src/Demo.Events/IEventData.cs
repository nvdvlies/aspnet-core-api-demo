namespace Demo.Events
{
    public interface IEventData
    {
        string EventDataVersion { get; }
        string CorrelationId { get; }
    }
}
