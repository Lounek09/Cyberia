using System.Text.Json;

namespace Cyberia.Utils;

public static class ExtendJsonElement
{
    public static int GetInt32OrDefault(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.Number)
        {
            return 0;
        }

        if (element.TryGetInt32(out var result))
        {
            return result;
        }

        return 0;
    }

    public static string GetStringOrDefault(this JsonElement element)
    {
        if (element.ValueKind is not JsonValueKind.String)
        {
            return string.Empty;
        }

        return element.GetString() ?? string.Empty;
    }
}
