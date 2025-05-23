﻿using Cyberia.Api.JsonConverters;
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
    /// Initializes a new instance of the <see cref="LocalizedString"/> struct with a default translation.
    /// </summary>
    /// <param name="default">The default translation.</param>
    public LocalizedString(string @default)
    {
        Default = @default;
        _translations = new()
        {
            { DofusApi.Config.BaseLanguage.ToStringFast(), @default }
        };
    }

    /// <summary>
    /// Adds a translation for the specified language if not already added.
    /// </summary>
    /// <param name="twoLetterISOLanguageName">The ISO 639-1 two-letter code for the language.</param>
    /// <param name="translation">The translation.</param>
    /// <returns><see langword="true"/> if the translation was added; otherwise, <see langword="false"/>.</returns>
    internal bool TryAdd(string twoLetterISOLanguageName, string translation)
    {
        return _translations.TryAdd(twoLetterISOLanguageName, translation);
    }

    /// <summary>
    /// Returns the translation for the specified language.
    /// </summary>
    /// <param name="twoLetterISOLanguageName">The ISO 639-1 two-letter code of the language to get the translation for.</param>
    /// <returns>The translation for the specified language; if not found, the default value.</returns>
    public string ToString(string twoLetterISOLanguageName)
    {
        if (_translations.TryGetValue(twoLetterISOLanguageName, out var translation) && !string.IsNullOrEmpty(translation))
        {
            return translation;
        }

        var defaultTwoLetterISOLanguageName = DofusApi.Config.SupportedLanguages[0].ToStringFast();
        if (_translations.TryGetValue(defaultTwoLetterISOLanguageName, out translation) && !string.IsNullOrEmpty(translation))
        {
            return translation;
        }

        return Default;
    }

    /// <inheritdoc cref="ToString(string)"/>
    /// <param name="language">The language to get the translation for.</param>
    public string ToString(Language language) => ToString(language.ToStringFast());

    /// <inheritdoc cref="ToString(string)"/>
    /// <param name="culture">The culture to get the translation for.</param>
    public string ToString(CultureInfo? culture)
    {
        return culture is null
            ? ToString()
            : ToString(culture.TwoLetterISOLanguageName);
    }

    /// <summary>
    /// Returns the translation for the current language.
    /// </summary>
    /// <returns>The translation for the current language.</returns>
    public override string ToString() => ToString(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

    public static implicit operator string(LocalizedString value) => value.ToString();
}
