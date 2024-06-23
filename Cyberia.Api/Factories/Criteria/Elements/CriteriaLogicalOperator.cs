namespace Cyberia.Api.Factories.Criteria;

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
    /// Gets the human-readable description of the logical operator.
    /// </summary>
    /// <returns>The description of the logical operator.</returns>
    public string GetDescription()
    {
        return Operator switch
        {
            '&' => ApiTranslations.Criterion_And,
            '|' => ApiTranslations.Criterion_Or,
            _ => Operator.ToString()
        };
    }
}
