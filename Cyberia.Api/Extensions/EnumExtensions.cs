using System.Globalization;

namespace Cyberia.Api.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T value, CultureInfo? culture = null)
        where T : Enum
    {
        if (Translation.TryGet<ApiTranslations>($"{typeof(T).Name}.{value}", out var description, culture))
        {
            return description;
        }

        return Translation.UnknownData(value, culture);
    }
}
