using System.Text;

namespace Cyberia.Translations;

/// <summary>
/// Provides methods to format strings using parameters.
/// </summary>
public static class Translation
{
    /// <summary>
    /// Formats the specified template using the provided parameter.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <param name="template">The template to format.</param>
    /// <param name="parameter">The parameter to use in the template.</param>
    /// <returns>The formatted string.</returns>
    public static string Format<T>(string template, T parameter)
    {
        return Format(template,
            parameter?.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Formats the specified template using the provided parameters.
    /// </summary>
    /// <typeparam name="T0">The type of the first parameter.</typeparam>
    /// <typeparam name="T1">The type of the second parameter.</typeparam>
    /// <param name="template">The template to format.</param>
    /// <param name="parameter0">The first parameter to use in the template.</param>
    /// <param name="parameter1">The second parameter to use in the template.</param>
    /// <returns>The formatted string.</returns>
    public static string Format<T0, T1>(string template, T0 parameter0, T1 parameter1)
    {
        return Format(template,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Formats the specified template using the provided parameters.
    /// </summary>
    /// <typeparam name="T0">The type of the first parameter.</typeparam>
    /// <typeparam name="T1">The type of the second parameter.</typeparam>
    /// <typeparam name="T2">The type of the third parameter.</typeparam>
    /// <param name="template">The template to format.</param>
    /// <param name="parameter0">The first parameter to use in the template.</param>
    /// <param name="parameter1">The second parameter to use in the template.</param>
    /// <param name="parameter2">The third parameter to use in the template.</param>
    /// <returns>The formatted string.</returns>
    public static string Format<T0, T1, T2>(string template, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return Format(template,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <summary>
    /// Formats the specified template using the provided parameters.
    /// </summary>
    /// <param name="template">The template to format.</param>
    /// <param name="parameters">The parameters to use in the template.</param>
    /// <returns>The formatted string.</returns>
    public static string Format(string template, params object[] parameters)
    {
        return Format(template, Array.ConvertAll(parameters, x => x?.ToString() ?? string.Empty));
    }

    /// <summary>
    /// Formats the specified template using the provided parameters.
    /// </summary>
    /// <param name="template">The template to format.</param>
    /// <param name="parameters">The parameters to use in the template.</param>
    /// <returns>The formatted string.</returns>
    public static string Format(string template, params string[] parameters) //TODO: .NET9 Use ReadOnlySpan<string>
    {
        StringBuilder builder = new(template);

        for (var i = 0; i < parameters.Length; i++)
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
            for (var i = 0; i < parameters.Length; i++)
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
