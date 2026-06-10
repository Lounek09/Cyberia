using System.Text.Json;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="JsonElement"/>.
/// </summary>
public static class JsonElementExtensions
{
    extension(JsonElement value)
    {
        /// <summary>
        /// Gets the boolean value of the <see cref="JsonElement"/>.
        /// </summary>
        /// <returns>The boolean value, or <see langword="default"/> if it's not a boolean.</returns>
        public bool GetBooleanOrDefault()
        {
            if (value.ValueKind is JsonValueKind.True)
            {
                return true;
            }

            return default;
        }

        /// <summary>
        /// Gets the value of the element as an <see cref="int"/> or <see langword="default"/> if it's not an integer.
        /// </summary>
        /// <returns>The integer value, or <see langword="default"/> if it's not an integer.</returns>
        public int GetInt32OrDefault()
        {
            if (value.ValueKind is not JsonValueKind.Number)
            {
                return default;
            }

            if (value.TryGetInt32(out var result))
            {
                return result;
            }

            return default;
        }

        /// <summary>
        /// Gets the value of the element as a <see cref="long"/> or <see langword="default"/> if it's not a long integer.
        /// </summary>
        /// <returns>The long integer value, or <see langword="default"/> if it's not a long integer.</returns>
        public long GetInt64OrDefault()
        {
            if (value.ValueKind is not JsonValueKind.Number)
            {
                return default;
            }

            if (value.TryGetInt64(out var result))
            {
                return result;
            }

            return default;
        }

        /// <summary>
        /// Gets the value of the element as a <see cref="string"/> or <see cref="string.Empty"/> if it's not a string.
        /// </summary>
        /// <returns>The string value or <see cref="string.Empty"/> if it's not a string.</returns>
        public string GetStringOrEmpty()
        {
            if (value.ValueKind is not JsonValueKind.String)
            {
                return string.Empty;
            }

            return value.GetString() ?? string.Empty;
        }
    }
}
