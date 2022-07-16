namespace Demo.Domain.Shared.DomainEntity;

public class ValidationMessage
{
    public ValidationMessage()
    {
    }

    public ValidationMessage(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }

    public string PropertyName { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return Message;
    }
}
