using System;

namespace Demo.Domain.Shared.Interfaces
{
    public interface ITimeZoneProvider
    {
        TimeZoneInfo TimeZone { get; }
    }
}
