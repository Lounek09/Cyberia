using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="INumber{TSelf}"/>.
/// </summary>
public static class INumberExtensions
{
    extension<T>(T value) where T : struct, INumber<T>
    {
        /// <summary>
        /// Converts a number to a formatted string with thousands separators.
        /// </summary>
        /// <param name="culture">The culture to use for formatting, or <see langword="null"/> to use the current culture.</param>
        /// <returns>The formatted string representation of the number.</returns>
        public string ToFormattedString(CultureInfo? culture = null)
        {
            return value.ToString("#,0", culture?.NumberFormat);
        }
    }

    extension(int value)
    {
        /// <summary>
        /// Gets the number of digits including the sign.
        /// </summary>
        /// <returns>The number of digits in the integer including the sign.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Length()
        {
            if (value == int.MinValue)
            {
                return 11;
            }

            var length = Math.Abs(value) switch
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

            return value < 0 ? length + 1 : length;
        }
    }
}
