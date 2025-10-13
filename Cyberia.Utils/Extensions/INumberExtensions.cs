using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="INumber{TSelf}"/>.
/// </summary>
public static class INumberExtensions
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

    /// <summary>
    /// Gets the number of digits including the sign.
    /// </summary>
    /// <param name="number">The integer to evaluate.</param>
    /// <returns>The number of digits in the integer including the sign.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Length(this int number)
    {
        if (number == int.MinValue)
        {
            return 11;
        }

        var length = Math.Abs(number) switch
        {
            < 10 => 1,
            < 100 => 2,
            < 1_000 => 3,
            < 10_000 => 4,
            < 100_000 => 5,
            < 1_000_000 => 6,
            < 10_000_000 => 7,
            < 100_000_000 => 8,
            < 1_000_000_000 => 9,
            _ => 10
        };

        return number < 0 ? length + 1 : length;
    }
}
