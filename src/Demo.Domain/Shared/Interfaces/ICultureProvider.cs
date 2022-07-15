using System.Globalization;

namespace Demo.Domain.Shared.Interfaces
{
    public interface ICultureProvider
    {
        CultureInfo Culture { get; }
    }
}
