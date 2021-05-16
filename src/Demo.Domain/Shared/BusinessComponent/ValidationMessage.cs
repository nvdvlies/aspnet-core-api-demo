namespace Demo.Domain.Shared.BusinessComponent
{
    public class ValidationMessage
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
