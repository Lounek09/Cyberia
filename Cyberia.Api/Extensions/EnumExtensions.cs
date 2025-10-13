using System.Globalization;

namespace Cyberia.Api.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Enum"/> interface.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the description from the translations.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="value">The value of the enum.</param>
    /// <param name="culture">The culture to use
    /// <returns></returns>
    public static string GetDescription<T>(this T value, CultureInfo? culture = null)
        where T : struct, Enum
    {
        var strValue = value.ToStringFast();

        if (Translation.TryGet<ApiTranslations>($"{typeof(T).Name}.{strValue}", out var description, culture))
        {
            return description;
        }

        return Translation.UnknownData(strValue, culture);
    }
}
