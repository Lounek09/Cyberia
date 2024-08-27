namespace Cyberia.Api.Factories.Criteria;

/// <summary>
/// Represents a criterion of an in game mechanic (to equip an item, if a spell effect is applicable, etc).
/// </summary>
public interface ICriterion : ICriteriaElement
{
    /// <summary>
    /// Gets the unique identifier of the criterion.
    /// </summary>
    string Id { get; init; }

    /// <summary>
    /// Gets the operator of the criterion.
    /// </summary>
    char Operator { get; init; }

    /// <summary>
    /// Generates a human-readable description of the criterion.
    /// </summary>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the criterion.</returns>
    DescriptionString GetDescription();
}
