using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Infrastructure.Auditlogging.Shared;

internal static class AuditlogFormatters
{
    public static string StringFormatter(string value)
    {
        return value;
    }

    public static string StringFormatter(Guid value)
    {
        if (value == Guid.Empty)
        {
            return null;
        }

        return value.ToString();
    }

    public static string BooleanFormatter(bool value)
    {
        return value ? "1" : "0";
    }

    public static string DecimalFormatter(decimal value)
    {
        return value.ToString("0.00");
    }

    public static string DateFormatter(DateTime value)
    {
        return value.ToString("o");
    }

    public static string DateFormatter(DateTime? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        return DateFormatter(value.Value);
    }

    public static string NumberFormatter(int value)
    {
        return value.ToString();
    }

    public static string NumberFormatter(long value)
    {
        return value.ToString();
    }

    public static string EnumFormatter(Enum value)
    {
        if (value == null)
        {
            return null;
        }

        return value.ToString("G");
    }

    public static string ListFormatter<T>(IList<T> value)
    {
        if (value == null || !value.Any())
        {
            return null;
        }

        return string.Join(",", value);
    }
}
