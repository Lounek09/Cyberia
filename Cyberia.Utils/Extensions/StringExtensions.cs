using System.Globalization;
using System.Numerics;

namespace Cyberia.Utils;

/// <summary>
/// Provides extension methods for <see cref="string"/>, <see cref="Span{T}"/> of <see cref="char"/> and <see cref="ReadOnlySpan{T}"/> of <see cref="char"/>.
/// </summary>
public static partial class StringExtensions
{
    /// <summary>
    /// Capitalizes the first character of the string.
    /// </summary>
    /// <param name="value">The string to capitalize.</param>
    /// <returns>The capitalized string.</returns>
    public static string Capitalize(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        if (value.Length == 1)
        {
            return char.ToUpper(value[0]).ToString();
        }

        return char.ToUpper(value[0]) + value[1..];
    }

    /// <inheritdoc cref="Capitalize(string)" />
    /// <param name="destination">The destination span.</param>
    /// <returns>The number of characters copied to the destination span.</returns>
    /// <exception cref="ArgumentException">Thorown when the destination span length is less than the source span length.</exception>
    public static int Capitalize(this ReadOnlySpan<char> value, Span<char> destination)
    {
        if (value.IsEmpty)
        {
            return 0;
        }

        if (destination.Length < value.Length)
        {
            throw new ArgumentException(
                $"The destination span length ({destination.Length}) must be greater than or equal to the source span length ({value.Length}).",
                nameof(destination));
        }

        destination[0] = char.ToUpper(value[0]);
        value[1..].CopyTo(destination[1..]);

        return value.Length;
    }

    /// <summary>
    /// Truncates the string to a maximum length.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the string.</param>
    /// <returns>The truncated string.</returns>
    public static string WithMaxLength(this string value, int maxLength)
    {
        return WithMaxLength(value.AsSpan(), maxLength).ToString();
    }

    /// <inheritdoc cref="WithMaxLength(string, int)" />
    public static ReadOnlySpan<char> WithMaxLength(this ReadOnlySpan<char> value, int maxLength)
    {
        if (value.Length <= maxLength)
        {
            return value;
        }

        return value[..maxLength];
    }

    /// <summary>
    /// Removes all occurrences of a specified string from the beginning of the string.
    /// </summary>
    /// <param name="value">The string to trim.</param>
    /// <param name="trimString">The string to remove.</param>
    /// <returns>The trimmed string.</returns>
    public static string TrimStart(this string value, ReadOnlySpan<char> trimString)
    {
        return TrimStart(value.AsSpan(), trimString).ToString();
    }

    /// <inheritdoc cref="TrimStart(string, ReadOnlySpan{char})" />
    public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> value, ReadOnlySpan<char> trimString)
    {
        if (value.IsEmpty)
        {
            return ReadOnlySpan<char>.Empty;
        }

        if (trimString.Length == 0)
        {
            return value;
        }

        while (value.StartsWith(trimString))
        {
            value = value[trimString.Length..];
        }

        return value;
    }

    /// <summary>
    /// Removes all occurrences of a specified string from the end of the string.
    /// </summary>
    /// <param name="value">The string to trim.</param>
    /// <param name="trimString">The string to remove.</param>
    /// <returns>The trimmed string.</returns>
    public static string TrimEnd(this string value, ReadOnlySpan<char> trimString)
    {
        return TrimEnd(value.AsSpan(), trimString).ToString();
    }

    /// <inheritdoc cref="TrimEnd(string, ReadOnlySpan{char})" />
    public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> value, ReadOnlySpan<char> trimString)
    {
        if (value.IsEmpty)
        {
            return ReadOnlySpan<char>.Empty;
        }

        if (trimString.Length == 0)
        {
            return value;
        }

        while (value.EndsWith(trimString))
        {
            value = value[..^trimString.Length];
        }

        return value;
    }

    /// <summary>
    /// Converts an hexadecimal string to a <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The targeted number type.</typeparam>
    /// <param name="value">The hexadecimal string to convert. A leading '-' indicates a negative number.</param>
    /// <returns>The parsed <typeparamref name="T"/>, or <c>0</c> if the conversion fails.</returns>
    public static T ToNumberOrZeroFromHex<T>(this string value)
        where T : INumber<T>
    {
        return ToNumberOrZeroFromHex<T>(value.AsSpan());
    }

    /// <inheritdoc cref="ToNumberOrZeroFromHex(string)" />
    public static T ToNumberOrZeroFromHex<T>(this ReadOnlySpan<char> value)
        where T : INumber<T>
    {
        if (value.IsEmpty)
        {
            return T.Zero;
        }

        var isNegative = value[0] == '-';
        if (isNegative)
        {
            value = value[1..];
        }

        if (!T.TryParse(value, NumberStyles.HexNumber, null, out var result))
        {
            return T.Zero;
        }

        return isNegative ? -result : result;
    }
}
