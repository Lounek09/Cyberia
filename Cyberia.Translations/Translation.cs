using Cyberia.Langzilla.Enums;

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cyberia.Translations;

/// <summary>
/// Provides methods to format strings using parameters.
/// </summary>
public static class Translation
{
    /// <summary>
    /// Returns the value of the string localized for the specified language.
    /// </summary>
    /// <typeparam name="T">The type of the translations wrapper.</typeparam>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="language">The language of the localized string.</param>
    /// <returns>The value of the string localized for the specified language, or the key if the resource is not found.</returns>
    public static string Get<T>(string key, Language language)
        where T : ITranslationsWrapper
    {
        return Get<T>(key, language.ToCulture());
    }

    /// <summary>
    /// Returns the value of the string localized for the specified culture.
    /// </summary>
    /// <typeparam name="T">The type of the translations wrapper.</typeparam>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="culture">The culture of the localized string, if not specified, the current UI culture is used.</param>
    /// <returns>The value of the string localized for the specified culture, or the key if the resource is not found.</returns>
    public static string Get<T>(string key, CultureInfo? culture = null)
        where T : ITranslationsWrapper
    {
        return T.ResourceManager.GetString(key, culture) ?? key;
    }

    /// <summary>
    /// Tries to get the value of the string localized for the specified language.
    /// </summary>
    /// <typeparam name="T">The type of the translations wrapper.</typeparam>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="language">The language of the localized string.</param>
    /// <param name="value">The value of the string localized for the specified language, or the key if the resource is not found.</param>
    /// <returns><see langword="true"/> if the string was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryGet<T>(string key, out string value, Language language)
        where T : ITranslationsWrapper
    {
        return TryGet<T>(key, out value, language.ToCulture());
    }

    /// <summary>
    /// Tries to get the value of the string localized for the specified culture.
    /// </summary>
    /// <typeparam name="T">The type of the translations wrapper.</typeparam>
    /// <param name="key">The key of the localized string.</param>
    /// <param name="value">The value of the string localized for the specified culture, or the key if the resource is not found.</param>
    /// <param name="culture">The culture of the localized string, if not specified, the current UI culture is used.</param
    /// <returns><see langword="true"/> if the string was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryGet<T>(string key, out string value, CultureInfo? culture = null)
        where T : ITranslationsWrapper
    {
        var nullableValue = T.ResourceManager.GetString(key, culture);

        if (nullableValue is null)
        {
            value = key;
            return false;
        }

        value = nullableValue;
        return true;
    }

    /// <summary>
    /// Returns a localized string indicating that the data is unknown for the specified language.
    /// </summary>
    /// <typeparam name="T">The type of the data identifier.</typeparam>
    /// <param name="id">The identifier of the data.</param>
    /// <param name="language">The language of the localized string.</param>
    /// <returns>The string localized for the specified language</returns>
    public static string UnknownData<T>(T id, Language language)
    {
        return UnknownData(id, language.ToCulture());
    }

    /// <summary>
    /// Returns a localized string indicating that the data is unknown for the specified culture.
    /// </summary>
    /// <typeparam name="T">The type of the data identifier.</typeparam>
    /// <param name="id">The identifier of the data.</param>
    /// <param name="culture">The culture of the localized string, if not specified, the current UI culture is used.</param>
    /// <returns>The string localized for the specified culture</returns>
    public static string UnknownData<T>(T id, CultureInfo? culture = null)
    {
        var template = Get<ApiTranslations>("Unknown.Data", culture);

        return Format(template, id);
    }

    /// <inheritdoc cref="Translation.Format(string, ReadOnlySpan{string})"/>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="parameter">The parameter to use in the template.</param>
    public static string Format<T>(string template, T parameter)
    {
        return Format(template,
            parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="Translation.Format(string, ReadOnlySpan{string})"/>
    /// <typeparam name="T0">The type of the first parameter.</typeparam>
    /// <typeparam name="T1">The type of the second parameter.</typeparam>
    /// <param name="parameter0">The first parameter to use in the template.</param>
    /// <param name="parameter1">The second parameter to use in the template.</param>
    public static string Format<T0, T1>(string template, T0 parameter0, T1 parameter1)
    {
        return Format(template,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="Translation.Format(string, ReadOnlySpan{string})"/>
    /// <typeparam name="T0">The type of the first parameter.</typeparam>
    /// <typeparam name="T1">The type of the second parameter.</typeparam>
    /// <typeparam name="T2">The type of the third parameter.</typeparam>
    /// <param name="parameter0">The first parameter to use in the template.</param>
    /// <param name="parameter1">The second parameter to use in the template.</param>
    /// <param name="parameter2">The third parameter to use in the template.</param>
    public static string Format<T0, T1, T2>(string template, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return Format(template,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="Translation.Format(string, ReadOnlySpan{string})"/>
    public static string Format(string template, params ReadOnlySpan<object?> parameters)
    {
        var length = parameters.Length;
        var strings = length > 0 ? new string[length] : Span<string>.Empty;
        for (var i = 0; i < length; i++)
        {
            strings[i] = parameters[i]?.ToString() ?? string.Empty;
        }

        return Format(template, strings);
    }

    /// <inheritdoc cref="Translation.Format(string, ReadOnlySpan{string})"/>
    [OverloadResolutionPriority(1)]
    public static string Format(string template, params IEnumerable<string> parameters)
    {
        return Format(template, parameters.ToArray());
    }

    /// <summary>
    /// Formats the specified template using the provided parameters.
    /// </summary>
    /// <param name="template">The template to format.</param>
    /// <param name="parameters">The parameters to use in the template.</param>
    /// <returns>The formatted string.</returns>
    [OverloadResolutionPriority(2)]
    public static string Format(string template, params ReadOnlySpan<string> parameters)
    {
        StringBuilder builder = new(template);

        var length = parameters.Length;
        for (var i = 0; i < length; i++)
        {
            builder.Replace($"#{i + 1}", parameters[i]);
        }

        var indexOfOpenBrace = template.IndexOf('{');
        while (indexOfOpenBrace != -1)
        {
            var indexOfCloseBrace = template.IndexOf('}', indexOfOpenBrace);
            if (indexOfCloseBrace == -1)
            {
                break;
            }

            var replacement = template[(indexOfOpenBrace + 1)..indexOfCloseBrace];
            for (var i = 0; i < length; i++)
            {
                if (!template[(indexOfOpenBrace + 1)..indexOfCloseBrace].Contains($"~{i + 1}"))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(parameters[i]))
                {
                    replacement = string.Empty;
                    break;
                }

                replacement = replacement.Replace($"~{i + 1}", string.Empty);
            }

            if (replacement.Contains('~'))
            {
                replacement = string.Empty;
            }

            builder.Replace(template[indexOfOpenBrace..(indexOfCloseBrace + 1)], replacement);

            indexOfOpenBrace = template.IndexOf('{', indexOfCloseBrace);
        }

        return builder.ToString();
    }
}
