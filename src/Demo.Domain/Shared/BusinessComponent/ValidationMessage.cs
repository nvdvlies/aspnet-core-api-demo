namespace Demo.Domain.Shared.BusinessComponent
{
    public class ValidationMessage
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }

        public ValidationMessage()
        {

        }

        public ValidationMessage(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
