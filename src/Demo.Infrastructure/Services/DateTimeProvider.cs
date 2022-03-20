using Demo.Common.Interfaces;
using System;

namespace Demo.Infrastructure.Services
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
