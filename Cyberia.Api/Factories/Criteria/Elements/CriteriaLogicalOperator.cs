using Cyberia.Langzilla.Primitives;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements;

/// <summary>
/// Represents a logical operator in a criteria.
/// </summary>
public sealed record CriteriaLogicalOperator : ICriteriaElement
{
    /// <summary>
    /// Gets the logical operator character.
    /// </summary>
    public char Operator { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CriteriaLogicalOperator"/> record.
    /// </summary>
    /// <param name="operator">The logical operator character.</param>
    internal CriteriaLogicalOperator(char @operator)
    {
        Operator = @operator;
    }

    /// <summary>
    /// Generates the human-readable description of the logical operator for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The description of the logical operator for the specified language.</returns>
    public string GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    /// <summary>
    /// Generates the human-readable description of the logical operator for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The description of the logical operator for the specified culture.</returns>
    public string GetDescription(CultureInfo? culture = null)
    {
        return Operator switch
        {
            '&' => Translation.Get<ApiTranslations>("Criterion.And", culture),
            '|' => Translation.Get<ApiTranslations>("Criterion.Or", culture),
            _ => Operator.ToString()
        };
    }
}
