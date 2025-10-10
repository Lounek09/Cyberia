using System.Text.Json;

namespace Cyberia.Utils.Extensions;

/// <summary>
/// Provides extension methods for <see cref="JsonElement"/>.
/// </summary>
public static class JsonElementExtensions
{
    /// <summary>
    /// Gets the boolean value of the <see cref="JsonElement"/>.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The boolean value, or <see langword="default"/> if it's not a boolean.</returns>
    public static bool GetBooleanOrDefault(this JsonElement element)
    {
        if (element.ValueKind is JsonValueKind.True)
        {
            return true;
        }

        return default;
    }

    /// <summary>
    /// Gets the integer value of the <see cref="JsonElement"/>.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The integer value, or <see langword="default"/> if it's not an integer.</returns>
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
    /// Gets the long integer value of the <see cref="JsonElement"/>.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The long integer value, or <see langword="default"/> if it's not a long integer.</returns>
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
    /// Gets the string value of the <see cref="JsonElement"/>.
    /// </summary>
    /// <param name="element">The JsonElement instance.</param>
    /// <returns>The string value or <see cref="string.Empty"/> if it's not a string.</returns>
    public static string GetStringOrEmpty(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.String)
        {
            return string.Empty;
        }

        return element.GetString() ?? string.Empty;
    }
}
