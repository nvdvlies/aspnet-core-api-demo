using Azure.Messaging.ServiceBus;
using Demo.Messages;
using System.Reflection;
using System.Text.Json;

namespace Demo.Infrastructure.Messages
{
    public static class Extensions
    {
        public static ServiceBusMessage ToServiceBusMessage(this IMessage message)
        {
            var assembly = typeof(Message<IMessage, IMessageData>).Assembly;
            var messageType = assembly.GetType(message.Type);
            var body = JsonSerializer.Serialize(message, messageType, new JsonSerializerOptions());
            return new ServiceBusMessage(body)
            {
                ContentType = message.Type
            };
        }

        public static IMessage ToMessage(this ServiceBusMessage serviceBusMessage)
        {
            var messageType = typeof(Message<IMessage, IMessageData>).Assembly.GetType(serviceBusMessage.ContentType);
            var methodName = nameof(Message<IMessage, IMessageData>.FromBinaryData);
            var method = messageType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var message2 = method.Invoke(null, new object[] { serviceBusMessage.Body });
            return (IMessage)message2;
        }
    }
}
