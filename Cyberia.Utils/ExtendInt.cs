using System.Globalization;

namespace Cyberia.Utils;

/// <summary>
/// Provides extension methods for int.
/// </summary>
public static class ExtendInt
{
    private static readonly NumberFormatInfo _numberFormatInfo = new()
    {
        NumberGroupSeparator = " ",
    };

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation using thousand separators.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <returns>A string formatted as the value with thousand separators.</returns>
    public static string ToStringThousandSeparator(this int value)
    {
        return value.ToString("#,0", _numberFormatInfo);
    }
}
