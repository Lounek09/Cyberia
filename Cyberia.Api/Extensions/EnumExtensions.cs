using System.Globalization;

namespace Cyberia.Api.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Enum"/> interface.
/// </summary>
public static class EnumExtensions
{
    extension<T>(T value) where T : struct, Enum
    {
        /// <summary>
        /// Gets the description of the enum from the translations for the specified culture.
        /// </summary>
        /// <param name="culture">The culture to use</param>
        /// <returns>The translated description of the enum.</returns>
        public string GetDescription(CultureInfo? culture = null)
        {
            var strValue = value.ToStringFast();

            if (Translation.TryGet<ApiTranslations>($"{typeof(T).Name}.{strValue}", out var description, culture))
            {
                return description;
            }

            return Translation.UnknownData(strValue, culture);
        }
    }
}
