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
    internal static readonly FrozenDictionary<Language, CultureInfo> _toCultures = new Dictionary<Language, CultureInfo>()
    {
        { Language.fr, new CultureInfo("fr") },
        { Language.en, new CultureInfo("en") },
        { Language.es, new CultureInfo("es") },
        { Language.de, new CultureInfo("de") },
        { Language.it, new CultureInfo("it") },
        { Language.nl, new CultureInfo("nl") },
        { Language.pt, new CultureInfo("pt") }
    }.ToFrozenDictionary();

    public static string ToStringFast(this Language language)
    {
        return Enum.GetName(language) ?? language.ToString();
    }

    /// <summary>
    /// Converts a <see cref="Language"/> to its corresponding <see cref="CultureInfo"/>.
    /// </summary>
    /// <param name="language">The language to convert.</param>
    /// <returns>The corresponding <see cref="CultureInfo"/>; if not found, the <see cref="CultureInfo"/> for <see cref="Language.en"/>.</returns>
    public static CultureInfo ToCulture(this Language language)
    {
        return _toCultures.TryGetValue(language, out var result) ? result : _toCultures[Language.en];
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
