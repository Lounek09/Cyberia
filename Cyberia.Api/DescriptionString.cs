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
    public IReadOnlyList<string> Parameters { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionString"/> struct.
    /// </summary>
    public DescriptionString()
    {
        Template = string.Empty;
        Parameters = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionString"/> struct.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="parameters">The parameters.</param>
    public DescriptionString(string template, params IReadOnlyList<string> parameters)
    {
        Template = template;
        Parameters = parameters;
    }

    /// <summary>
    /// Returns a string formed by the template populated with its parameters.
    /// </summary>
    /// <returns>The formatted string.</returns>
    public override string ToString() => Translation.Format(Template, Parameters);

    /// <inheritdoc cref="ToString()"/>
    /// <param name="decorator">The decorator function to apply to each parameter.</param>
    public string ToString(Func<string, string> decorator)
    {
        var length = Parameters.Count;
        var formattedParameters = length > 0 ? new string[length] : Span<string>.Empty;
        for (var i = 0; i < length; i++)
        {
            var parameter = Parameters[i];
            formattedParameters[i] = string.IsNullOrEmpty(parameter) ? parameter : decorator(parameter);
        }

        return Translation.Format(Template, formattedParameters);
    }

    public static implicit operator string(DescriptionString description) => description.ToString();
}
