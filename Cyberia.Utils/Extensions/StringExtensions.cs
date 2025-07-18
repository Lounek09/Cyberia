﻿using System.Globalization;
using System.Numerics;

namespace Cyberia.Utils;

/// <summary>
/// Provides extension methods for String and ReadOnlySpan<char>.
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
        return Capitalize(value.AsSpan());
    }

    /// <inheritdoc cref="Capitalize(string)" />
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

    /// <inheritdoc cref="WithMaxLength(string, int)" />
    public static string WithMaxLength(this ReadOnlySpan<char> value, int maxLength)
    {
        if (value.Length <= maxLength)
        {
            return value.ToString();
        }

        return value[..maxLength].ToString();
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

    /// <inheritdoc cref="TrimStart(string, ReadOnlySpan{char})" />
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

    /// <inheritdoc cref="TrimEnd(string, ReadOnlySpan{char})" />
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
    /// Converts an hexadecimal string to a <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The targeted number type.</typeparam>
    /// <param name="value">The hexadecimal string to convert. An optional leading '-' indicates a negative number.</param>
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
