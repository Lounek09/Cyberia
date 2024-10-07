using System.Collections.Frozen;
using System.Globalization;

namespace Cyberia.Langzilla.Enums;

/// <summary>
/// Language of the lang.
/// </summary>
public enum LangLanguage
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
/// Provides extension methods for the <see cref="LangLanguage"/> enum.
/// </summary>
public static class LangLanguageExtensions
{
    internal static readonly FrozenDictionary<LangLanguage, CultureInfo> _toCultures = new Dictionary<LangLanguage, CultureInfo>()
    {
        { LangLanguage.fr, new CultureInfo("fr") },
        { LangLanguage.en, new CultureInfo("en") },
        { LangLanguage.es, new CultureInfo("es") },
        { LangLanguage.de, new CultureInfo("de") },
        { LangLanguage.it, new CultureInfo("it") },
        { LangLanguage.nl, new CultureInfo("nl") },
        { LangLanguage.pt, new CultureInfo("pt") }
    }.ToFrozenDictionary();

    public static string ToStringFast(this LangLanguage language)
    {
        return Enum.GetName(language) ?? language.ToString();
    }

    /// <summary>
    /// Converts a <see cref="LangLanguage"/> to its corresponding <see cref="CultureInfo"/>.
    /// </summary>
    /// <param name="language">The language to convert.</param>
    /// <returns>The corresponding <see cref="CultureInfo"/>; if not found, the <see cref="CultureInfo"/> for <see cref="LangLanguage.en"/>.</returns>
    public static CultureInfo ToCulture(this LangLanguage language)
    {
        return _toCultures.TryGetValue(language, out var result) ? result : _toCultures[LangLanguage.en];
    }

    /// <summary>
    /// Converts a <see cref="CultureInfo"/> to its corresponding <see cref="LangLanguage"/>.
    /// </summary>
    /// <param name="culture">The culture to convert.</param>
    /// <returns>The corresponding <see cref="LangLanguage"/>; if not found, <see cref="LangLanguage.en"/>.</returns>
    public static LangLanguage ToLangLanguage(this CultureInfo culture)
    {
        return Enum.TryParse<LangLanguage>(culture.TwoLetterISOLanguageName, out var result) ? result : LangLanguage.en;
    }
}
