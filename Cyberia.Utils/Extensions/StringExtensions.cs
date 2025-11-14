using System.Globalization;
using System.Numerics;

namespace Cyberia.Utils;

/// <summary>
/// Provides extension methods for <see cref="string"/>, <see cref="Span{T}"/> of <see cref="char"/> and <see cref="ReadOnlySpan{T}"/> of <see cref="char"/>.
/// </summary>
public static partial class StringExtensions
{
    extension(string value)
    {
        /// <summary>
        /// Capitalizes the first character of the string.
        /// </summary>
        /// <returns>The capitalized string.</returns>
        public string Capitalize()
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

        /// <summary>
        /// Truncates the string to a maximum length.
        /// </summary>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <returns>The truncated string.</returns>
        public string WithMaxLength(int maxLength)
        {
            return WithMaxLength(value.AsSpan(), maxLength).ToString();
        }

        /// <summary>
        /// Removes all occurrences of a specified string from the beginning of the string.
        /// </summary>
        /// <param name="trimString">The string to remove.</param>
        /// <returns>The trimmed string.</returns>
        public string TrimStart(ReadOnlySpan<char> trimString)
        {
            return TrimStart(value.AsSpan(), trimString).ToString();
        }

        /// <summary>
        /// Removes all occurrences of a specified string from the end of the string.
        /// </summary>
        /// <param name="trimString">The string to remove.</param>
        /// <returns>The trimmed string.</returns>
        public string TrimEnd(ReadOnlySpan<char> trimString)
        {
            return TrimEnd(value.AsSpan(), trimString).ToString();
        }

        /// <summary>
        /// Converts an hexadecimal string to a <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The targeted number type.</typeparam>
        /// <returns>The parsed <typeparamref name="T"/>, or <c>0</c> if the conversion fails.</returns>
        public T ToNumberOrZeroFromHex<T>()
            where T : INumber<T>
        {
            return ToNumberOrZeroFromHex<T>(value.AsSpan());
        }
    }

    extension(ReadOnlySpan<char> value)
    {
        /// <inheritdoc cref="Capitalize(string)" />
        /// <param name="destination">The destination span.</param>
        /// <returns>The number of characters copied to the destination span.</returns>
        /// <exception cref="ArgumentException">Thorown when the destination span length is less than the source span length.</exception>
        public int Capitalize(Span<char> destination)
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

        /// <inheritdoc cref="WithMaxLength(string, int)" />
        public ReadOnlySpan<char> WithMaxLength(int maxLength)
        {
            if (value.Length <= maxLength)
            {
                return value;
            }

            return value[..maxLength];
        }

        /// <inheritdoc cref="TrimStart(string, ReadOnlySpan{char})" />
        public ReadOnlySpan<char> TrimStart(ReadOnlySpan<char> trimString)
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

        /// <inheritdoc cref="TrimEnd(string, ReadOnlySpan{char})" />
        public ReadOnlySpan<char> TrimEnd(ReadOnlySpan<char> trimString)
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

        /// <inheritdoc cref="ToNumberOrZeroFromHex(string)" />
        public T ToNumberOrZeroFromHex<T>()
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
}
