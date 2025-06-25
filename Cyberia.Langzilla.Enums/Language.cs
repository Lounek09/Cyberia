using System.Collections.Frozen;
using System.Globalization;

namespace Cyberia.Langzilla.Enums;

/// <summary>
/// Language of the lang.
/// </summary>
public enum Language
{
    /// <summary>
    /// French.
    /// </summary>
    fr,
    /// <summary>
    /// English.
    /// </summary>
    en,
    /// <summary>
    /// Spanish.
    /// </summary>
    es,
    /// <summary>
    /// Deutsch.
    /// </summary>
    de,
    /// <summary>
    /// Italian.
    /// </summary>
    it,
    /// <summary>
    /// Netherlands.
    /// </summary>
    nl,
    /// <summary>
    /// Portuguese.
    /// </summary>
    pt
}

/// <summary>
/// Provides extension methods for the <see cref="Language"/> enum.
/// </summary>
public static class LanguageExtensions
{
    private static readonly FrozenDictionary<Language, CultureInfo> s_cachedCultureByLanguage = new Dictionary<Language, CultureInfo>()
    {
        { Language.fr, CultureInfo.GetCultureInfo("fr") },
        { Language.en, CultureInfo.GetCultureInfo("en") },
        { Language.es, CultureInfo.GetCultureInfo("es") },
        { Language.de, CultureInfo.GetCultureInfo("de") },
        { Language.it, CultureInfo.GetCultureInfo("it") },
        { Language.nl, CultureInfo.GetCultureInfo("nl") },
        { Language.pt, CultureInfo.GetCultureInfo("pt") }
    }.ToFrozenDictionary();

    /// <summary>
    /// Converts a <see cref="Language"/> to its corresponding <see cref="CultureInfo"/>.
    /// </summary>
    /// <param name="language">The language to convert.</param>
    /// <returns>The corresponding <see cref="CultureInfo"/>; if not found, the <see cref="CultureInfo"/> for <see cref="Language.en"/>.</returns>
    public static CultureInfo ToCulture(this Language language)
    {
        s_cachedCultureByLanguage.TryGetValue(language, out var result);
        return result ?? s_cachedCultureByLanguage[Language.en];
    }

    /// <summary>
    /// Converts a <see cref="CultureInfo"/> to its corresponding <see cref="Language"/>.
    /// </summary>
    /// <param name="culture">The culture to convert.</param>
    /// <returns>The corresponding <see cref="Language"/>; if not found, <see cref="Language.en"/>.</returns>
    public static Language ToLangLanguage(this CultureInfo culture)
    {
        return Enum.TryParse<Language>(culture.TwoLetterISOLanguageName, out var result) ? result : Language.en;
    }
}
