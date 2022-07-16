using System.Linq;
using System.Reflection;

namespace Demo.SignalrTypescript.Generator.Models;

internal class Event
{
    public Event(MethodInfo methodInfo)
    {
        EventName = methodInfo.Name; // CustomerCreated
        ClassName = string.Concat(methodInfo.Name, "Event"); // CustomerCreatedEvent
        Parameters = methodInfo
            .GetParameters()
            .Select(parameterInfo => new Parameter(parameterInfo))
            .ToArray();
    }

    public string EventName { get; }
    public string ClassName { get; }
    public Parameter[] Parameters { get; }
}
