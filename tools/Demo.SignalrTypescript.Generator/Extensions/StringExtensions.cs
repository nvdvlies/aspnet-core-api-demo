namespace Demo.SignalrTypescript.Generator.Extensions;

internal static class StringExtensions
{
    public static string ToCamelCase(this string value)
    {
        return string.IsNullOrEmpty(value) ? value : string.Concat(char.ToLowerInvariant(value[0]), value[1..]);
    }
}
