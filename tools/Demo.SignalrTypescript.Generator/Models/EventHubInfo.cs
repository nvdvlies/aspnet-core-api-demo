using Demo.Application.Shared.Interfaces;
using System;
using System.Linq;

namespace Demo.SignalrTypescript.Generator.Models
{
    class EventHubInfo
    {
        public EventHub[] EventHubs { get; }

        public EventHubInfo(Type eventHubInterface)
        {
            EventHubs = eventHubInterface
                .GetInterfaces()
                .Select(type => new EventHub(type))
                .ToArray();
        }
    }
}
