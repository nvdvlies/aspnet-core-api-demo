using Demo.Messages.Invoice;

namespace Demo.Infrastructure.Events
{
    public class RabbitMqEvent
    {
        public string ContentType { get; set; }
        public string Payload { get; set; }
    }
}