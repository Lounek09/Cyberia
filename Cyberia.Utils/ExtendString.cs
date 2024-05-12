using System.Globalization;

namespace Cyberia.Utils;

/// <summary>
/// Provides extension methods for String.
/// </summary>
public static partial class ExtendString
{
    /// <summary>
    /// Capitalizes the first character of the string.
    /// </summary>
    /// <param name="value">The string to capitalize.</param>
    /// <returns>The capitalized string.</returns>
    public static string Capitalize(this string value)
    {
        return Capitalize(value.AsSpan());
    }

    /// <summary>
    /// Capitalizes the first character of the ReadOnlySpan.
    /// </summary>
    /// <param name="value">The ReadOnlySpan to capitalize.</param>
    /// <returns>The capitalized ReadOnlySpan as a string.</returns>
    public static string Capitalize(this ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return string.Empty;
        }

        if (value.Length > 1)
        {
            return char.ToUpper(value[0]) + value[1..].ToString();
        }

        return value.ToString().ToUpper();
    }

    /// <summary>
    /// Truncates the string to a maximum length.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the string.</param>
    /// <returns>The truncated string.</returns>
    public static string WithMaxLength(this string value, int maxLength)
    {
        return WithMaxLength(value.AsSpan(), maxLength);
    }

    /// <summary>
    /// Truncates the ReadOnlySpan to a maximum length.
    /// </summary>
    /// <param name="value">The ReadOnlySpan to truncate.</param>
    /// <param name="maxLength">The maximum length of the ReadOnlySpan.</param>
    /// <returns>The truncated ReadOnlySpan as a string.</returns>
    public static string WithMaxLength(this ReadOnlySpan<char> value, int maxLength)
    {
        if (value.Length <= maxLength)
        {
            return value.ToString();
        }

        return value[..maxLength].ToString();
    }

    /// <summary>
    /// Splits the string into substrings of a specified length.
    /// </summary>
    /// <param name="value">The string to split.</param>
    /// <param name="length">The length of each substring.</param>
    /// <returns>An IEnumerable of substrings.</returns>
    public static IEnumerable<string> SplitByLength(this string value, int length)
    {
        if (length <= 0)
        {
            yield return value;
            yield break;
        }

        for (var i = 0; i < value.Length; i += length)
        {
            yield return value.Substring(i, Math.Min(length, value.Length - i));
        }
    }

    /// <summary>
    /// Removes all occurrences of a specified string from the beginning of the string.
    /// </summary>
    /// <param name="value">The string to trim.</param>
    /// <param name="trimString">The string to remove.</param>
    /// <returns>The trimmed string.</returns>
    public static string TrimStart(this string value, ReadOnlySpan<char> trimString)
    {
        return TrimStart(value.AsSpan(), trimString);
    }

    /// <summary>
    /// Removes all occurrences of a specified ReadOnlySpan from the beginning of the ReadOnlySpan.
    /// </summary>
    /// <param name="value">The ReadOnlySpan to trim.</param>
    /// <param name="trimString">The ReadOnlySpan to remove.</param>
    /// <returns>The trimmed ReadOnlySpan as a string.</returns>
    public static string TrimStart(this ReadOnlySpan<char> value, ReadOnlySpan<char> trimString)
    {
        if (value.IsEmpty)
        {
            return string.Empty;
        }

        if (trimString.Length == 0)
        {
            return value.ToString();
        }

        while (value.StartsWith(trimString))
        {
            value = value[trimString.Length..];
        }

        return value.ToString();
    }

    /// <summary>
    /// Removes all occurrences of a specified string from the end of the string.
    /// </summary>
    /// <param name="value">The string to trim.</param>
    /// <param name="trimString">The string to remove.</param>
    /// <returns>The trimmed string.</returns>
    public static string TrimEnd(this string value, ReadOnlySpan<char> trimString)
    {
        return TrimEnd(value.AsSpan(), trimString);
    }

    /// <summary>
    /// Removes all occurrences of a specified ReadOnlySpan from the end of the ReadOnlySpan.
    /// </summary>
    /// <param name="value">The ReadOnlySpan to trim.</param>
    /// <param name="trimString">The ReadOnlySpan to remove.</param>
    /// <returns>The trimmed ReadOnlySpan as a string.</returns>
    public static string TrimEnd(this ReadOnlySpan<char> value, ReadOnlySpan<char> trimString)
    {
        if (value.IsEmpty)
        {
            return string.Empty;
        }

        if (trimString.Length == 0)
        {
            return value.ToString();
        }

        while (value.EndsWith(trimString))
        {
            value = value[..^trimString.Length];
        }

        return value.ToString();
    }

    /// <summary>
    /// Converts a hexadecimal string to a long integer, or returns the default value if the conversion fails.
    /// </summary>
    /// <param name="value">The hexadecimal string to convert.</param>
    /// <returns>The converted long integer, or the default value if the conversion fails.</returns>
    public static long ToInt64OrDefaultFromHex(this string value)
    {
        return ToInt64OrDefaultFromHex(value.AsSpan());
    }

    /// <summary>
    /// Converts a hexadecimal ReadOnlySpan to a long integer, or returns the default value if the conversion fails.
    /// </summary>
    /// <param name="value">The hexadecimal ReadOnlySpan to convert.</param>
    /// <returns>The converted long integer, or the default value if the conversion fails.</returns>
    public static long ToInt64OrDefaultFromHex(this ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return default;
        }

        var isNegative = value[0] == '-';
        value = isNegative ? value[1..] : value;

        if (long.TryParse(value, NumberStyles.HexNumber, null, out var result))
        {
            return isNegative ? -result : result;
        }

        return default;
    }
}
