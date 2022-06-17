using System;

namespace Demo.Domain.Shared.Exceptions
{
    public class DomainEntityNotFoundException : Exception
    {
        public DomainEntityNotFoundException()
        {
        }

        public DomainEntityNotFoundException(string message)
            : base(message)
        {
        }

        public DomainEntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}