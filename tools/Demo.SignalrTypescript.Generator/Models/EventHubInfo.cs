using System;
using System.Linq;

namespace Demo.SignalrTypescript.Generator.Models;

internal class EventHubInfo
{
    public EventHubInfo(Type eventHubInterface)
    {
        EventHubs = eventHubInterface
            .GetInterfaces()
            .Select(type => new EventHub(type))
            .ToArray();
    }

    public EventHub[] EventHubs { get; }
}
