using System;
using System.Globalization;

namespace Demo.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        TimeZoneInfo TimeZone { get; }
        CultureInfo Culture { get; }
    }
}
