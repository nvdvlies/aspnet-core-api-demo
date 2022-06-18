using System;
using System.Linq;

namespace Demo.SignalrTypescript.Generator.Models
{
    internal class EventHub
    {
        public EventHub(Type @interface)
        {
            ClassName = @interface.Name[1..].Replace("EventHub", "Events"); // ICustomerEventHub -> CustomerEvents
            Events = @interface
                .GetMethods()
                .Select(methodInfo => new Event(methodInfo))
                .ToArray();
        }

        public string ClassName { get; }
        public Event[] Events { get; }
    }
}
