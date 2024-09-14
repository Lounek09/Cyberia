using System.Text.Json;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for JsonElement.
/// </summary>
public static class JsonElementExtensions
{
    /// <summary>
    /// Gets the integer value of the <see cref="JsonElement"/>, or the <see langword="default"/> value if it's not a number.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The integer value.</returns>
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
    /// Gets the long integer value of the <see cref="JsonElement"/>, or the <see langword="default"/> value if it's not a number.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The long integer value.</returns>
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
    /// Gets the string value of the <see cref="JsonElement">, or <see cref="string.Empty"/> if it's not a string.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The string value.</returns>
    public static string GetStringOrEmpty(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.String)
        {
            return string.Empty;
        }

        return element.GetString() ?? string.Empty;
    }
}
