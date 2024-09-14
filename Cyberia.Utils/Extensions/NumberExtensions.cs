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
    /// <returns>A string formatted as the value with thousand separators.</returns>
    public static string ToFormattedString(this int value)
    {
        return value.ToString("#,0");
    }

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation using thousand separators.
    /// </summary>
    /// <param name="value">The long value.</param>
    /// <returns>A string formatted as the value with thousand separators.</returns>
    public static string ToFormattedString(this long value)
    {
        return value.ToString("#,0");
    }
}
