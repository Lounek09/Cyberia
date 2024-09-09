using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api;

/// <summary>
/// Represents a string with translations.
/// </summary>
[JsonConverter(typeof(LocalizedStringConverter))]
public readonly record struct LocalizedString
{
    public static readonly LocalizedString Empty = new();

    private readonly Dictionary<string, string> _translations;

    /// <summary>
    /// Gets the default translation.
    /// </summary>
    public string Default { get; init; }

    /// <summary>
    /// Gets the translations.
    /// </summary>
    public IReadOnlyDictionary<string, string> Translations => _translations.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizedString"/> struct.
    /// </summary>
    public LocalizedString()
    {
        Default = string.Empty;
        _translations = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizedString"/> struct.
    /// </summary>
    /// <param name="default">The default translation.</param>
    public LocalizedString(string @default)
    {
        Default = @default;
        _translations = [];
    }

    /// <summary>
    /// Adds a translation for the specified language if not already added.
    /// </summary>
    /// <param name="twoLetterISOLanguageName">The ISO 639-1 two-letter code for the language.</param>
    /// <param name="translation">The translation.</param>
    /// <returns><see langword="true"/> if the translation was added; otherwise, <see langword="false"/>.</returns>
    internal bool Add(string twoLetterISOLanguageName, string translation)
    {
        if (_translations.ContainsKey(twoLetterISOLanguageName))
        {
            return false;
        }

        _translations.Add(twoLetterISOLanguageName, translation);
        return true;
    }

    /// <inheritdoc cref="Add(string, string)"/>
    /// <param name="culture">The culture.</param>
    internal void Add(CultureInfo culture, string translation) => Add(culture.TwoLetterISOLanguageName, translation);

    /// <inheritdoc cref="Add(string, string)"/>
    /// <param name="language">The language.</param>
    internal void Add(LangLanguage language, string translation) => Add(language.ToCulture(), translation);

    /// <summary>
    /// Returns the translation for the specified language.
    /// </summary>
    /// <param name="twoLetterISOLanguageName">The ISO 639-1 two-letter code for the language.</param>
    /// <returns>The translation for the specified language; if not found, the default value.</returns>
    public string ToString(string twoLetterISOLanguageName)
    {
        if (_translations.TryGetValue(twoLetterISOLanguageName, out var translation) && !string.IsNullOrEmpty(translation))
        {
            return translation;
        }

        var defaultLanguage = DofusApi.Config.SupportedLanguages[0].ToCulture().TwoLetterISOLanguageName;
        if (_translations.TryGetValue(defaultLanguage, out translation) && !string.IsNullOrEmpty(translation))
        {
            return translation;
        }

        return Default;
    }

    /// <inheritdoc cref="ToString(string)"/>
    /// <param name="culture">The culture.</param>
    public string ToString(CultureInfo culture) => ToString(culture.TwoLetterISOLanguageName);

    /// <inheritdoc cref="ToString(string)"/>
    /// <param name="language">The language.</param>
    public string ToString(LangLanguage language) => ToString(language.ToCulture());

    /// <summary>
    /// Returns the translation for the current language.
    /// </summary>
    /// <returns>The translation for the current language; if not found, the default value.</returns>
    public override string ToString() => ToString(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

    public static implicit operator string(LocalizedString value) => value.ToString();
}
