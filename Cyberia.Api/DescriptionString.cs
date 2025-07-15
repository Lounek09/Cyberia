using System.Collections.ObjectModel;

namespace Cyberia.Api;

/// <summary>
/// Represents a string template with parameters.
/// </summary>
public readonly record struct DescriptionString
{
    /// <summary>
    /// Gets an empty description.
    /// </summary>
    public static readonly DescriptionString Empty = new();

    /// <summary>
    /// Gets the template.
    /// </summary>
    public string Template { get; init; }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    public ReadOnlyCollection<string> Parameters => _parameters.AsReadOnly();

    private readonly string[] _parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionString"/> struct.
    /// </summary>
    public DescriptionString()
    {
        Template = string.Empty;
        _parameters = Array.Empty<string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionString"/> struct.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="parameters">The parameters.</param>
    public DescriptionString(string template, params string[] parameters)
    {
        Template = template;
        _parameters = parameters;
    }

    /// <summary>
    /// Returns a string formed by the template populated with its parameters.
    /// </summary>
    /// <returns>The formatted string.</returns>
    public override string ToString()
    {
        if (_parameters.Length == 0)
        {
            return Template;
        }

        return Translation.Format(Template, _parameters);
    }

    /// <inheritdoc cref="ToString()"/>
    /// <param name="decorator">The decorator function to apply to each parameter.</param>
    public string ToString(Func<string, string> decorator)
    {
        var length = _parameters.Length;
        if (length == 0)
        {
            return Template;
        }

        var formattedParameters = new string[length];
        for (var i = 0; i < length; i++)
        {
            var parameter = _parameters[i];
            formattedParameters[i] = string.IsNullOrEmpty(parameter) ? parameter : decorator(parameter);
        }

        return Translation.Format(Template, formattedParameters);
    }

    public static implicit operator string(DescriptionString description) => description.ToString();
}
