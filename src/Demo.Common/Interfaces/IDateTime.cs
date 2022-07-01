using System;

namespace Demo.Common.Interfaces
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}