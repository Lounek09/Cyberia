using System.Globalization;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for numbers.
/// </summary>
public static class NumberExtensions
{
    /// <summary>
    /// Converts the value of this instance to its equivalent string representation using thousand separators.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <param name="culture">The culture to use for formatting.</param>
    /// <returns>A string formatted as the value with thousand separators.</returns>
    public static string ToFormattedString(this int value, CultureInfo? culture = null)
    {
        return value.ToString("#,0", culture?.NumberFormat);
    }

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation using thousand separators.
    /// </summary>
    /// <param name="value">The long value.</param>
    /// <param name="culture">The culture to use for formatting.</param>
    /// <returns>A string formatted as the value with thousand separators.</returns>
    public static string ToFormattedString(this long value, CultureInfo? culture = null)
    {
        return value.ToString("#,0", culture?.NumberFormat);
    }
}
