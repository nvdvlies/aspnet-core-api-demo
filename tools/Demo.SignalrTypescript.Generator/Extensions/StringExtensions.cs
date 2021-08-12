namespace Demo.SignalrTypescript.Generator.Extensions
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return string.Concat(char.ToLowerInvariant(value[0]), value[1..]);
        }
    }
}
