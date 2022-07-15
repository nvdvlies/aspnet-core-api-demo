using System;
using Demo.Common.Interfaces;

namespace Demo.Infrastructure.Services
{
    internal class DateTimeProvider : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
