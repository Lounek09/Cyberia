using System.Globalization;
using System.Numerics;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for numbers.
/// </summary>
public static class NumberExtensions
{
    /// <summary>
    /// Converts a number to a formatted string with thousands separators.
    /// </summary>
    /// <typeparam name="T">The type of the number.</typeparam>
    /// <param name="value">The number to format.</param>
    /// <param name="culture">The culture to use for formatting, or <see langword="null"/> to use the current culture.</param>
    /// <returns>The formatted string representation of the number.</returns>
    public static string ToFormattedString<T>(this T value, CultureInfo? culture = null)
        where T : struct, INumber<T>
    {
        return value.ToString("#,0", culture?.NumberFormat);
    }
}
