using System;

namespace Demo.Domain.Shared.Exceptions
{
    public static class Guard
    {
        public static void NotNull(object value, string paramName, string message = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static void NotNullOrWhiteSpace(string value, string paramName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static void NotTrue(bool value, string paramName, string message = null)
        {
            if (value)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }
    }
}
