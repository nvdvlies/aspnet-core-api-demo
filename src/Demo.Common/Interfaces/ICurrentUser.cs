using System;
using System.Globalization;

namespace Demo.Common.Interfaces
{
    public interface ICurrentUser
    {
        string Id { get; }
        TimeZoneInfo TimeZone { get; }
        CultureInfo Culture { get; }
    }
}
