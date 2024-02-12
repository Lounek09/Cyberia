using System.Text.Json;

namespace Cyberia.Utils;

public static class ExtendJsonElement
{
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

    public static string GetStringOrEmpty(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.String)
        {
            return string.Empty;
        }

        return element.GetString() ?? string.Empty;
    }
}
