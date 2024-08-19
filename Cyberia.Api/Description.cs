namespace Cyberia.Api;

/// <summary>
/// Represents a string template with parameters.
/// </summary>
public readonly record struct Description
{
    /// <summary>
    /// Gets an empty description.
    /// </summary>
    public static readonly Description Empty = new();

    private readonly string[] _parameters;

    /// <summary>
    /// Gets the template.
    /// </summary>
    public string Template { get; init; }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    public IReadOnlyList<string> Parameters => _parameters.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="Description"/> struct.
    /// </summary>
    public Description()
    {
        Template = string.Empty;
        _parameters = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Description"/> struct.
    /// </summary>
    /// <param name="template">The template.</param>
    /// <param name="parameters">The parameters.</param>
    public Description(string template, params string[] parameters)
    {
        Template = template;
        _parameters = parameters;
    }

    /// <summary>
    /// Returns a string formed by the template populated with its parameters.
    /// </summary>
    /// <returns>The formatted string.</returns>
    public override string ToString() => Translation.Format(Template, _parameters);

    /// <inheritdoc cref="ToString()"/>
    /// <param name="decorator">The decorator function to apply to each parameter.</param>
    public string ToString(Func<string, string> decorator)
    {
        return Translation.Format(Template, Array.ConvertAll(_parameters, x => string.IsNullOrEmpty(x) ? x : decorator(x)));
    }

    public static implicit operator string(Description description) => description.ToString();
}
