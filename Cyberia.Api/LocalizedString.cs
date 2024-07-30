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
    /// <param name="default">The default translation.</param>
    public LocalizedString(string @default)
    {
        Default = @default;
        _translations = [];
    }

    /// <summary>
    /// Adds a translation for the specified language if not already added.
    /// </summary>
    /// <param name="cultureName">The name of the culture.</param>
    /// <param name="translation">The translation.</param>
    /// <returns><see langword="true"/> if the translation was added; otherwise, <see langword="false"/>.</returns>
    internal bool Add(string cultureName, string translation)
    {
        if (_translations.ContainsKey(cultureName))
        {
            return false;
        }

        _translations.Add(cultureName, translation);
        return true;
    }

    /// <inheritdoc cref="Add(string, string)"/>
    /// <param name="culture">The culture.</param>
    internal void Add(CultureInfo culture, string translation) => Add(culture.Name, translation);

    /// <inheritdoc cref="Add(string, string)"/>
    /// <param name="language">The language.</param>
    internal void Add(LangLanguage language, string translation) => Add(language.ToCulture().Name, translation);

    /// <summary>
    /// Returns the translation for the specified language.
    /// </summary>
    /// <param name="cultureName">The name of the culture.</param>
    /// <returns>The translation for the specified language; if not found, the default value.</returns>
    public string ToString(string cultureName)
    {
        return _translations.TryGetValue(cultureName, out var translation)
            ? translation
            : Default;
    }

    /// <inheritdoc cref="ToString(string)"/>
    /// <param name="culture">The culture.</param>
    public string ToString(CultureInfo culture) => ToString(culture.Name);

    /// <inheritdoc cref="ToString(string)"/>
    /// <param name="language">The language.</param>
    public string ToString(LangLanguage language) => ToString(language.ToCulture());

    /// <summary>
    /// Returns the translation for the current language.
    /// </summary>
    /// <returns>The translation for the current language; if not found, the default value.</returns>
    public override string ToString() => ToString(CultureInfo.CurrentCulture.Name);

    public static implicit operator string(LocalizedString value) => value.ToString();
}
