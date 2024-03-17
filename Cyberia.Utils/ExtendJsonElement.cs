using System.Text.Json;

namespace Cyberia.Utils;

/// <summary>
/// Provides extension methods for JsonElement.
/// </summary>
public static class ExtendJsonElement
{
    /// <summary>
    /// Gets the integer value of the JsonElement, or the default value if it's not a number.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The integer value or default.</returns>
    public static int GetInt32OrDefault(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.Number)
        {
            return default;
        }

        if (element.TryGetInt32(out var result))
        {
            return result;
        }

        return default;
    }

    /// <summary>
    /// Gets the long integer value of the JsonElement, or the default value if it's not a number.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The long integer value or default.</returns>
    public static long GetInt64OrDefault(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.Number)
        {
            return default;
        }

        if (element.TryGetInt64(out var result))
        {
            return result;
        }

        return default;
    }

    /// <summary>
    /// Gets the string value of the JsonElement, or an empty string if it's not a string.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The string value or an empty string.</returns>
    public static string GetStringOrEmpty(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.String)
        {
            return string.Empty;
        }

        return element.GetString() ?? string.Empty;
    }
}
