using System;
using Demo.Common.Interfaces;

namespace Demo.Infrastructure.Services
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}