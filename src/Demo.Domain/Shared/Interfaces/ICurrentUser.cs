using System;
using System.Globalization;

namespace Demo.Domain.Shared.Interfaces
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        TimeZoneInfo TimeZone { get; }
        CultureInfo Culture { get; }
    }
}
